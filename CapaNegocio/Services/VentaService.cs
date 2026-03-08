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
                    var negocio = _context.Negocios.FirstOrDefault();
                    int ptoVenta = negocio?.PuntoVenta ?? 1;

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

                    foreach (var detDto in dto.Detalles)
                    {
                        var producto = _context.Productos.FirstOrDefault(p => p.Id == detDto.ProductoId);

                        if (producto == null)
                            throw new Exception($"El producto '{detDto.ProductoNombre}' no existe.");

                        if (producto.Stock < detDto.Cantidad)
                            throw new Exception($"Stock insuficiente para el producto '{detDto.ProductoNombre}'. Disponible: {producto.Stock}");

                        producto.Stock -= detDto.Cantidad;

                        ventaEntity.DetallesVenta.Add(new DetalleVenta
                        {
                            ProductoId = detDto.ProductoId,
                            Cantidad = detDto.Cantidad,
                            PrecioVenta = detDto.PrecioVenta,
                            SubTotal = detDto.SubTotal
                        });
                    }

                    _context.Ventas.Add(ventaEntity);
                    _context.SaveChanges();

                    transaction.Commit();
                    mensaje = numeroDocumentoFinal;
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
            var query = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Where(v => v.FechaRegistro.Date >= fechaInicio.Date && v.FechaRegistro.Date <= fechaFin.Date);

            if (clienteId.HasValue && clienteId > 0)
            {
                query = query.Where(v => v.ClienteId == clienteId);
            }

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
            .OrderByDescending(v => v.FechaRegistro)
            .ToList();
        }
        public bool Desactivar(int idVenta, out string mensaje)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var venta = _context.Ventas
                        .Include(v => v.DetallesVenta)
                        .FirstOrDefault(v => v.Id == idVenta);

                    if (venta == null)
                    {
                        mensaje = "La venta no existe.";
                        return false;
                    }

                    foreach (var detalle in venta.DetallesVenta)
                    {
                        var producto = _context.Productos.Find(detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.Stock += detalle.Cantidad;
                        }
                    }

                    _context.Ventas.Remove(venta);

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
            var v = _context.Ventas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(v => v.NumeroDocumento == nroDocumento);

            if (v == null) return null;

            return new VentaDetalleDto
            {
                TipoDocumento = v.TipoDocumento,
                NumeroDocumento = v.NumeroDocumento,
                FechaRegistro = v.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                UsuarioNombre = v.Usuario.NombreCompleto,
                DocumentoCliente = v.Cliente.Documento,
                NombreCliente = v.Cliente.NombreCompleto,

                MontoTotal = v.MontoTotal,
                MontoPago = v.MontoPago,
                MontoCambio = v.MontoCambio,
                MetodoPago = v.MetodoPago,

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
