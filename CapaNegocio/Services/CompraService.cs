using System;
using System.Collections.Generic;
using System.Text;
using CapaNegocio.DTOs;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;

namespace CapaNegocio.Services
{
    public class CompraService
    {
        private AppDbContext _context;
        public CompraService(AppDbContext context)
        {
            _context = context;
        }
        public bool Registrar(CompraCreateDto dto, out string mensaje)
        {
            mensaje = string.Empty;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Obtenemos el Punto de Venta configurado
                    var negocio = _context.Negocios.FirstOrDefault();
                    int ptoVenta = negocio?.PuntoVenta ?? 1;

                    // 2. Buscamos la última compra para generar el correlativo
                    // Filtramos las que empiecen con nuestro punto de venta
                    var ultimaCompra = _context.Compras
                        .Where(c => c.NumeroDocumento.StartsWith(ptoVenta.ToString("D4")))
                        .OrderByDescending(c => c.Id)
                        .Select(c => c.NumeroDocumento)
                        .FirstOrDefault();

                    int nuevoCorrelativo = 1;

                    if (!string.IsNullOrEmpty(ultimaCompra))
                    {
                        // Usamos LastIndexOf por si el nombre del negocio o algo raro tuviera otro guion
                        int posicionGuion = ultimaCompra.LastIndexOf('-');

                        if (posicionGuion != -1)
                        {
                            string parteNumerica = ultimaCompra.Substring(posicionGuion + 1);
                            if (int.TryParse(parteNumerica, out int ultimoNro))
                            {
                                nuevoCorrelativo = ultimoNro + 1;
                            }
                        }
                    }

                    string numeroDocumentoFinal = $"{ptoVenta:D4}-{nuevoCorrelativo:D8}";
                    dto.NumeroDocumento = numeroDocumentoFinal;
                    // 3. Mapeo y Guardado
                    var compraEntity = new Compra
                    {
                        UsuarioId = dto.UsuarioId,
                        ProveedorId = dto.ProveedorId,
                        TipoDocumento = dto.TipoDocumento,
                        NumeroDocumento = numeroDocumentoFinal, // El número "Real"
                        MetodoPago = dto.MetodoPago,
                        MontoTotal = dto.MontoTotal,
                        Estado = true,
                        FechaRegistro = DateTime.Now,
                        // Aquí iría el mapeo de DetallesCompra si lo haces manual o con AutoMapper
                    };

                    compraEntity.DetallesCompra = dto.Detalles.Select(d => new DetalleCompra
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioCompra = d.PrecioCompra,
                        PrecioVenta = d.PrecioVenta, // Si lo manejas en la compra
                        MontoTotal = d.MontoTotal
                    }).ToList();

                    foreach (var det in compraEntity.DetallesCompra)
                    {
                        // 1. Buscamos el producto en la base de datos
                        var producto = _context.Productos.FirstOrDefault(p => p.Id == det.ProductoId);

                        if (producto != null)
                        {
                            // 2. ACTUALIZAMOS PRECIOS (Siempre el último valor ingresado)
                            producto.PrecioCompra = det.PrecioCompra;
                            producto.PrecioVenta = det.PrecioVenta;

                            // 3. ACTUALIZAMOS STOCK (Sumamos lo que compramos)
                            producto.Stock += det.Cantidad;
                        }
                    }

                    _context.Compras.Add(compraEntity);
                    _context.SaveChanges();

                    transaction.Commit();
                    mensaje = numeroDocumentoFinal;
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensaje = "Error: " + ex.Message;
                    return false;
                }
            }
        }
        public List<CompraListadoDto> Listar()
        {
            var compras = _context.Compras
                .Where(p => p.Estado == true)
                .Include(p => p.Proveedor)
                .Include(u => u.Usuario)
                .Select(c => new CompraListadoDto
                {
                    Id = c.Id,
                    NumeroDocumento = c.NumeroDocumento,
                    TipoDocumento = c.TipoDocumento,
                    MetodoPago = c.MetodoPago,
                    RazonSocialProveedor = c.Proveedor.RazonSocial,
                    NombreUsuario = c.Usuario.NombreCompleto,
                    MontoTotal = c.MontoTotal,
                    FechaRegistro = c.FechaRegistro
                })
            .OrderByDescending(c => c.FechaRegistro)
            .ToList();
            return compras;
        }
        public List<CompraListadoDto> Buscar(DateTime fechaInicio, DateTime fechaFin, int? proveedorId = null)
        {
            var query = _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Where(c => c.FechaRegistro.Date >= fechaInicio.Date && c.FechaRegistro.Date <= fechaFin.Date);

            if (proveedorId.HasValue && proveedorId > 0)
            {
                query = query.Where(c => c.ProveedorId == proveedorId);
            }

            return query.Select(c => new CompraListadoDto
            {
                Id = c.Id,
                NumeroDocumento = c.NumeroDocumento,
                TipoDocumento = c.TipoDocumento,
                MetodoPago = c.MetodoPago,
                RazonSocialProveedor = c.Proveedor.RazonSocial,
                NombreUsuario = c.Usuario.NombreCompleto,
                MontoTotal = c.MontoTotal,
                FechaRegistro = c.FechaRegistro
            }).ToList();
        }
        public bool Desactivar(int idCompra, out string mensaje)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Traemos la compra con sus detalles
                    var compra = _context.Compras
                        .Include(c => c.DetallesCompra)
                        .FirstOrDefault(c => c.Id == idCompra);

                    if (compra == null)
                    {
                        mensaje = "La compra no existe.";
                        return false;
                    }

                    if (!compra.Estado)
                    {
                        mensaje = "Esta compra ya se encuentra desactivada/anulada.";
                        return false;
                    }

                    // 1. REVERSIÓN DE STOCK
                    // Como la compra se "cancela", debemos restar lo que entró.
                    foreach (var detalle in compra.DetallesCompra)
                    {
                        var producto = _context.Productos.Find(detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.Stock -= detalle.Cantidad;

                            // Seguridad: Evitar stock negativo si ya se vendió parte de esa compra
                            if (producto.Stock < 0) producto.Stock = 0;
                        }
                    }

                    // 2. BORRADO LÓGICO
                    compra.Estado = false;

                    _context.SaveChanges();
                    transaction.Commit();

                    mensaje = "La compra ha sido desactivada y el stock revertido.";
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    mensaje = "Error al desactivar: " + ex.Message;
                    return false;
                }
            }
        }

        public Compra GetById(int idCompra)
        {
            return _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.DetallesCompra)
                .ThenInclude(d => d.Producto)
                .FirstOrDefault(c => c.Id == idCompra);
        }

        public CompraDetalleDto ObtenerDetalle(string nroDocumento)
        {
            // Buscamos la compra con todos sus datos relacionados
            var c = _context.Compras
                .AsNoTracking() // Optimización para lectura
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Include(c => c.DetallesCompra)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(c => c.NumeroDocumento == nroDocumento);

            if (c == null) return null;

            // Mapeamos manualmente a tu CompraDetalleDto
            return new CompraDetalleDto
            {
                TipoDocumento = c.TipoDocumento,
                NumeroDocumento = c.NumeroDocumento,
                MetodoPago = c.MetodoPago, // Aquí se mapea el medio de pago que mencionaste
                MontoTotal = c.MontoTotal,
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                UsuarioNombre = c.Usuario.NombreCompleto,
                DocumentoProveedor = c.Proveedor.Documento,
                RazonSocial = c.Proveedor.RazonSocial,

                // Mapeamos la lista de detalles
                Detalles = c.DetallesCompra.Select(d => new DetalleCompraCreateDto
                {
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    Codigo = d.Producto.Codigo,
                    PrecioCompra = d.PrecioCompra,
                    PrecioVenta = d.PrecioVenta,
                    MontoTotal = d.MontoTotal
                }).ToList()
            };
        }
        public List<string> ObtenerPuntosDeVenta()
        {
            var desdeCompras = _context.Compras
                .Select(c => c.NumeroDocumento.Split('-', StringSplitOptions.None)[0])
                .Distinct()
                .ToList();

            var puntoVentaNegocio = _context.Negocios
                .Select(n => n.PuntoVenta.ToString("D4"))
                .FirstOrDefault();

            if (puntoVentaNegocio != null && !desdeCompras.Contains(puntoVentaNegocio))
                desdeCompras.Add(puntoVentaNegocio);

            return desdeCompras.OrderBy(x => x).ToList();
        }
    }
}
