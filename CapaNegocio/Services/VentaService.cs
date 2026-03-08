using CapaEntidad.Domain.Entities;
using CapaNegocio.DTOs;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.Services
{
    public class VentaService
    {
        private AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public bool Registrar(VentaCreateDto dto, out string mensaje)
        {
            mensaje = string.Empty;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Obtener Punto de Venta configurado
                    var negocio = _context.Negocios.FirstOrDefault();
                    int ptoVenta = negocio?.PuntoVenta ?? 1;

                    // 2. Generar Correlativo de Venta (similar a compras)
                    var ultimaVenta = _context.Ventas
                        .Where(v => v.NumeroDocumento.StartsWith(ptoVenta.ToString("D4")))
                        .OrderByDescending(v => v.Id)
                        .Select(v => v.NumeroDocumento)
                        .FirstOrDefault();

                    int nuevoCorrelativo = 1;
                    if (!string.IsNullOrEmpty(ultimaVenta))
                    {
                        int posicionGuion = ultimaVenta.LastIndexOf('-');
                        if (posicionGuion != -1)
                        {
                            string parteNumerica = ultimaVenta.Substring(posicionGuion + 1);
                            if (int.TryParse(parteNumerica, out int ultimoNro))
                            {
                                nuevoCorrelativo = ultimoNro + 1;
                            }
                        }
                    }

                    string numeroDocumentoFinal = $"{ptoVenta:D4}-{nuevoCorrelativo:D8}";
                    dto.NumeroDocumento = numeroDocumentoFinal;
                    // 3. Mapeo de la Entidad Venta
                    var ventaEntity = new Venta
                    {
                        UsuarioId = dto.UsuarioId,
                        ClienteId = dto.ClienteId,
                        TipoDocumento = dto.TipoDocumento,
                        NumeroDocumento = numeroDocumentoFinal,
                        MontoPago = dto.MontoPago,
                        MetodoPago = dto.MetodoPago,
                        MontoCambio = dto.MontoCambio,
                        MontoTotal = dto.MontoTotal,
                        FechaRegistro = DateTime.Now
                    };

                    // 4. Mapeo de Detalles y Validación de Stock
                    foreach (var detDto in dto.Detalles)
                    {
                        // Buscamos el producto en la DB
                        var producto = _context.Productos.FirstOrDefault(p => p.Id == detDto.ProductoId);

                        if (producto == null)
                            throw new Exception($"El producto '{detDto.ProductoNombre}' no existe.");

                        // VALIDACIÓN CRÍTICA: ¿Hay stock?
                        if (producto.Stock < detDto.Cantidad)
                            throw new Exception($"Stock insuficiente para el producto '{detDto.ProductoNombre}'. Disponible: {producto.Stock}");

                        // Restamos el stock
                        producto.Stock -= detDto.Cantidad;

                        // Agregamos el detalle a la entidad
                        ventaEntity.DetallesVenta.Add(new DetalleVenta
                        {
                            ProductoId = detDto.ProductoId,
                            Cantidad = detDto.Cantidad,
                            PrecioVenta = detDto.PrecioVenta, // Guardamos el precio al que se vendió
                            SubTotal = detDto.SubTotal
                        });
                    }

                    // 5. Guardar en Base de Datos
                    _context.Ventas.Add(ventaEntity);
                    _context.SaveChanges();

                    transaction.Commit();
                    mensaje = numeroDocumentoFinal; // Retornamos el número generado
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensaje = ex.Message;
                    return false;
                }
            }
        }
        public List<VentaListadoDto> Listar()
        {
            var ventas = _context.Ventas
                .Include(v => v.Cliente)
                .Include(u => u.Usuario)
                .Select(v => new VentaListadoDto
                {
                    Id = v.Id,
                    NumeroDocumento = v.NumeroDocumento,
                    TipoDocumento = v.TipoDocumento,
                    NombreCliente = v.Cliente.NombreCompleto,
                    DocumentoCliente = v.Cliente.Documento,
                    NombreUsuario = v.Usuario.NombreCompleto,
                    MontoTotal = v.MontoTotal,
                    FechaRegistro = v.FechaRegistro
                })
                .OrderByDescending(v => v.FechaRegistro)
                .ToList();

            return ventas;
        }
        public List<VentaListadoDto> Buscar(DateTime fechaInicio, DateTime fechaFin, int? clienteId = null)
        {
            // 1. Iniciamos la consulta con los Includes necesarios
            var query = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Where(v => v.FechaRegistro.Date >= fechaInicio.Date && v.FechaRegistro.Date <= fechaFin.Date);

            // 2. Filtramos por cliente solo si se proporcionó un ID válido
            if (clienteId.HasValue && clienteId > 0)
            {
                query = query.Where(v => v.ClienteId == clienteId);
            }

            // 3. Proyectamos al DTO de listado de ventas
            return query.Select(v => new VentaListadoDto
            {
                Id = v.Id,
                NumeroDocumento = v.NumeroDocumento,
                TipoDocumento = v.TipoDocumento,
                NombreCliente = v.Cliente.NombreCompleto,
                DocumentoCliente = v.Cliente.Documento,
                NombreUsuario = v.Usuario.NombreCompleto,
                MontoTotal = v.MontoTotal,
                FechaRegistro = v.FechaRegistro
            })
            .OrderByDescending(v => v.FechaRegistro) // Siempre las más recientes primero
            .ToList();
        }
        public bool Desactivar(int idVenta, out string mensaje)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Traemos la venta con sus detalles
                    var venta = _context.Ventas
                        .Include(v => v.DetallesVenta)
                        .FirstOrDefault(v => v.Id == idVenta);

                    if (venta == null)
                    {
                        mensaje = "La venta no existe.";
                        return false;
                    }

                    // Si manejas un campo Estado en Ventas (opcional, pero recomendado)
                    // if (!venta.Estado) { ... }

                    // 1. REVERSIÓN DE STOCK
                    // Como la venta se anula, el producto "vuelve" al negocio.
                    foreach (var detalle in venta.DetallesVenta)
                    {
                        var producto = _context.Productos.Find(detalle.ProductoId);
                        if (producto != null)
                        {
                            // Devolvemos al stock lo que se había llevado el cliente
                            producto.Stock += detalle.Cantidad;
                        }
                    }

                    // 2. BORRADO LÓGICO O ANULACIÓN
                    // Podés borrarla físicamente, pero en ERPs se prefiere cambiar un estado 
                    // para no perder la correlatividad de los números de factura.
                    _context.Ventas.Remove(venta); // O venta.Estado = false si agregas el campo

                    _context.SaveChanges();
                    transaction.Commit();

                    mensaje = "La venta ha sido anulada y los productos reintegrados al stock.";
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensaje = "Error al anular la venta: " + ex.Message;
                    return false;
                }
            }
        }
        public VentaDetalleDto ObtenerDetalle(string nroDocumento)
        {
            // 1. Buscamos la venta con sus relaciones (Cliente, Usuario y Detalles con Producto)
            var v = _context.Ventas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(v => v.NumeroDocumento == nroDocumento);

            if (v == null) return null;

            // 2. Mapeamos al DTO de detalle de venta
            return new VentaDetalleDto
            {
                TipoDocumento = v.TipoDocumento,
                NumeroDocumento = v.NumeroDocumento,
                FechaRegistro = v.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                UsuarioNombre = v.Usuario.NombreCompleto,
                DocumentoCliente = v.Cliente.Documento,
                NombreCliente = v.Cliente.NombreCompleto,

                // Campos específicos de Venta
                MontoTotal = v.MontoTotal,
                MontoPago = v.MontoPago,
                MontoCambio = v.MontoCambio,
                MetodoPago = v.MetodoPago,

                // Mapeamos la lista de detalles (usando tu DetalleVentaCreateDto corregido)
                Detalles = v.DetallesVenta.Select(d => new DetalleVentaCreateDto
                {
                    ProductoId = d.ProductoId,
                    Codigo = d.Producto.Codigo,
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioVenta = d.PrecioVenta,
                    SubTotal = d.SubTotal
                }).ToList()
            };
        }

        public List<string> ObtenerPuntosDeVenta()
        {
            var desdeVentas = _context.Ventas
                .Select(c => c.NumeroDocumento.Split('-', StringSplitOptions.None)[0])
                .Distinct()
                .ToList();

            var puntoVentaNegocio = _context.Negocios
                .Select(n => n.PuntoVenta.ToString("D4"))
                .FirstOrDefault();

            if (puntoVentaNegocio != null && !desdeVentas.Contains(puntoVentaNegocio))
                desdeVentas.Add(puntoVentaNegocio);

            return desdeVentas.OrderBy(x => x).ToList();
        }
    }
}
