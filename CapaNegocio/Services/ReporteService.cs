using CapaNegocio.DTOs;
using CapaNegocio.DTOs.CapaNegocio.DTOs.Reportes;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.Services
{
    public class ReporteService
    {
        private readonly AppDbContext _context;

        public ReporteService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. REPORTE DE VENTAS ---
        public List<ReporteVentaDto> ObtenerVentas(DateTime fechaInicio, DateTime fechaFin, string metodoPago)
        {
            var query = _context.Ventas
            .AsNoTracking()
            .Include(v => v.Usuario)
            .Include(v => v.Cliente)
            .Include(v => v.DetallesVenta)
            .ThenInclude(d => d.Producto)
            .Where(v => v.FechaRegistro.Date >= fechaInicio.Date && v.FechaRegistro.Date <= fechaFin.Date);

            // FILTRO NUEVO: Si no elige "Todos", filtramos por el método seleccionado
            if (metodoPago != "Todos")
            {
                query = query.Where(v => v.MetodoPago == metodoPago);
            }

            return query.Select(v => new ReporteVentaDto
            {
                IdVenta = v.Id,
                FechaRegistro = v.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                NumeroDocumento = v.NumeroDocumento,
                TipoDocumento = v.TipoDocumento,
                Usuario = v.Usuario.NombreCompleto,
                DocumentoCliente = v.Cliente.Documento,
                NombreCliente = v.Cliente.NombreCompleto,
                MetodoPago = v.MetodoPago,
                MontoPago = v.MontoPago,
                MontoCambio = v.MontoCambio,
                MontoTotal = v.MontoTotal,
                // Tu lógica de ganancia impecable
                GananciaVenta = v.MontoTotal - v.DetallesVenta.Sum(d => d.Cantidad * d.Producto.PrecioCompra)
            }).ToList();
        }

        // --- 2. REPORTE DE COMPRAS ---
        public List<ReporteCompraDto> ObtenerCompras(DateTime fechaInicio, DateTime fechaFin, int idProveedor = 0)
        {
            var query = _context.DetallesCompra
                .AsNoTracking()
                .Include(d => d.Compra)
                    .ThenInclude(c => c.Usuario)
                .Include(d => d.Compra)
                    .ThenInclude(c => c.Proveedor)
                .Include(d => d.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(d => d.Compra.FechaRegistro.Date >= fechaInicio.Date &&
                            d.Compra.FechaRegistro.Date <= fechaFin.Date);

            // Filtro por Proveedor (si seleccionan uno específico en el combo)
            if (idProveedor != 0)
            {
                query = query.Where(d => d.Compra.ProveedorId == idProveedor);
            }

            return query.Select(d => new ReporteCompraDto
            {
                IdCompra = d.CompraId,
                FechaRegistro = d.Compra.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                NumeroDocumento = d.Compra.NumeroDocumento,
                TipoDocumento = d.Compra.TipoDocumento,
                UsuarioRegistro = d.Compra.Usuario.NombreCompleto,
                MontoTotal = d.Compra.MontoTotal,
                DocumentoProveedor = d.Compra.Proveedor.Documento,
                RazonSocial = d.Compra.Proveedor.RazonSocial,
                CodigoProducto = d.Producto.Codigo,
                NombreProducto = d.Producto.Nombre,
                Categoria = d.Producto.Categoria.Descripcion,
                PrecioCompra = d.PrecioCompra,
                PrecioVenta = d.PrecioVenta,
                Cantidad = d.Cantidad,
                SubTotal = d.Cantidad * d.PrecioCompra// O d.Cantidad * d.PrecioCompra
            }).ToList();
        }

        // --- 3. REPORTE DE STOCK ---
        public List<ReporteStockDto> ObtenerStock()
        {
            return _context.Productos
                .AsNoTracking()
                .Where(p => p.Estado == true)
                .Include(p => p.Categoria)
                .Select(p => new ReporteStockDto
                {
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    Categoria = p.Categoria.Descripcion,
                    Stock = p.Stock,
                    PrecioCompra = p.PrecioCompra,
                    PrecioVenta = p.PrecioVenta
                }).ToList();
        }

        // --- 4. REPORTE RESUMEN (DASHBOARD) ---
        public ReporteResumenDto ObtenerResumen(DateTime fechaInicio, DateTime fechaFin)
        {
            // 1. Traemos las ventas y compras del periodo (usamos ToList para no castigar a la DB con múltiples consultas)
            var ventasActual = _context.Ventas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Where(v => v.FechaRegistro.Date >= fechaInicio.Date && v.FechaRegistro.Date <= fechaFin.Date)
                .ToList();

            var comprasActual = _context.Compras
                .AsNoTracking()
                .Include(c => c.Proveedor)
                .Where(c => c.FechaRegistro.Date >= fechaInicio.Date && c.FechaRegistro.Date <= fechaFin.Date)
                .ToList();

            TimeSpan diferencia = fechaFin - fechaInicio;
            DateTime inicioAnterior = fechaInicio.AddDays(-(diferencia.Days + 1));
            DateTime finAnterior = fechaInicio.AddDays(-1);

            var comprasAnterior = _context.Compras.AsNoTracking()
                .Where(c => c.FechaRegistro.Date >= inicioAnterior.Date && c.FechaRegistro.Date <= finAnterior.Date)
                .ToList();

            var ventasAnterior = _context.Ventas.AsNoTracking()
                .Where(v => v.FechaRegistro.Date >= inicioAnterior.Date && v.FechaRegistro.Date <= finAnterior.Date)
                .ToList();

            // 3. Totales y Tickets para Variaciones
            decimal totalComprasActual = comprasActual.Sum(c => c.MontoTotal);
            decimal totalComprasAnterior = comprasAnterior.Sum(c => c.MontoTotal);

            decimal totalActual = ventasActual.Sum(v => v.MontoTotal);
            decimal totalAnterior = ventasAnterior.Sum(v => v.MontoTotal);

            decimal ticketActual = ventasActual.Count > 0 ? totalActual / ventasActual.Count : 0;
            decimal ticketAnterior = ventasAnterior.Count > 0 ? totalAnterior / ventasAnterior.Count : 0;

            // 2. Cálculo del Producto Estrella (Más vendido por cantidad)
            var productoEstrella = _context.DetallesVenta
        .AsNoTracking()
        .Include(d => d.Producto)
        .Where(d => d.FechaRegistro.Date >= fechaInicio.Date && d.FechaRegistro.Date <= fechaFin.Date)
        .GroupBy(d => d.Producto.Nombre)
        .OrderByDescending(g => g.Sum(x => x.Cantidad))
        .Select(g => new { Nombre = g.Key, Total = g.Sum(x => x.Cantidad) })
        .FirstOrDefault();

            // 3. Cálculo del Cliente Frecuente
            var clienteFrecuente = ventasActual
        .GroupBy(v => v.Cliente?.NombreCompleto ?? "Anónimo")
        .OrderByDescending(g => g.Count())
        .Select(g => g.Key).FirstOrDefault() ?? "N/A";

            // 4. Cálculo del Método de Pago Preferido
            var metodoPreferido = ventasActual
                .GroupBy(v => v.MetodoPago)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "N/A";

            // Cambia la línea que te dio el error por esta:
            var provPrincipal = comprasActual
        .Where(c => c.Proveedor != null)
        .GroupBy(c => c.Proveedor.RazonSocial)
        .OrderByDescending(g => g.Sum(x => x.MontoTotal))
        .Select(g => g.Key).FirstOrDefault() ?? "Sin Proveedor";

            // Cantidad de artículos promedio por ticket
            var totalArticulos = _context.DetallesVenta
        .Where(d => d.Venta.FechaRegistro >= fechaInicio && d.Venta.FechaRegistro <= fechaFin)
        .Sum(d => (int?)d.Cantidad) ?? 0;

            // Día con más ventas
            var diaPico = ventasActual
        .GroupBy(v => v.FechaRegistro.Date)
        .OrderByDescending(g => g.Sum(x => x.MontoTotal))
        .Select(g => new { Fecha = g.Key.ToShortDateString(), Monto = g.Sum(x => x.MontoTotal) })
        .FirstOrDefault();

            // 5. Armamos el DTO final con TODA la lógica
            var resumen = new ReporteResumenDto
            {
                TotalVentas = totalActual,
                TotalCompras = totalComprasActual,
                CantidadVentas = ventasActual.Count,
                CantidadCompras = comprasActual.Count,
                CantidadClientes = ventasActual.Select(v => v.ClienteId).Distinct().Count(),
                ProductosBajoStock = _context.Productos.Count(p => p.Stock <= 5),
                ProductoMasVendido = productoEstrella?.Nombre ?? "Sin ventas",
                UnidadesProductoMasVendido = productoEstrella?.Total ?? 0,
                ClienteFrecuente = clienteFrecuente,
                MetodoPagoPreferido = metodoPreferido,
                ProveedorPrincipal = provPrincipal,
                ArticulosPorTicket = ventasActual.Count > 0 ? (double)totalArticulos / ventasActual.Count : 0,
                DiaPicoVenta = diaPico != null ? $"{diaPico.Fecha} ({diaPico.Monto:C0})" : "N/A",
                VariacionVentas = CalcularVariacion(totalActual, totalAnterior),
                VariacionTicket = CalcularVariacion(ticketActual, ticketAnterior),
                VariacionCompras = CalcularVariacion(totalComprasActual, totalComprasAnterior),
                VariacionCantidadVentas = CalcularVariacion(ventasActual.Count, (decimal)ventasAnterior.Count),
                FechaInicio = fechaInicio.ToShortDateString(),
                FechaFin = fechaFin.ToShortDateString()
            };
            for (DateTime fecha = fechaInicio.Date; fecha <= fechaFin.Date; fecha = fecha.AddDays(1))
            {
                var vDia = ventasActual.Where(v => v.FechaRegistro.Date == fecha).Sum(v => v.MontoTotal);
                var cDia = comprasActual.Where(c => c.FechaRegistro.Date == fecha).Sum(c => c.MontoTotal);

                resumen.VentasPorDia.Add(vDia);
                resumen.ComprasPorDia.Add(cDia);
                resumen.EtiquetasDias.Add(fecha.ToString("dd/MM"));
            }

            // 4. FINALMENTE DEVOLVEMOS EL OBJETO YA LLENO
            return resumen;
        }
        private decimal CalcularVariacion(decimal actual, decimal anterior)
        {
            if (anterior == 0) return actual > 0 ? 100 : 0;
            return ((actual - anterior) / anterior) * 100;
        }
        public VentaDetalleDto ObtenerDetalleVentaCompleto(int idVenta)
        {
            var venta = _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(v => v.Id == idVenta);

            if (venta == null) return null;

            return new VentaDetalleDto
            {
                TipoDocumento = venta.TipoDocumento,
                NumeroDocumento = venta.NumeroDocumento,
                FechaRegistro = venta.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                UsuarioNombre = venta.Usuario.NombreCompleto,
                DocumentoCliente = venta.Cliente.Documento,
                NombreCliente = venta.Cliente.NombreCompleto,
                MetodoPago = venta.MetodoPago,
                MontoTotal = venta.MontoTotal,
                MontoPago = venta.MontoPago,
                MontoCambio = venta.MontoCambio,
                // Mapeamos la lista de detalles usando tu DetalleVentaCreateDto
                Detalles = venta.DetallesVenta.Select(d => new DetalleVentaCreateDto
                {
                    ProductoNombre = d.Producto.Nombre,
                    PrecioVenta = d.PrecioVenta,
                    Cantidad = d.Cantidad,
                    SubTotal = d.SubTotal
                }).ToList()
            };
        }
        public decimal ObtenerTotalMesAnterior()
        {
            DateTime inicioMesPasado = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime finMesPasado = inicioMesPasado.AddMonths(1).AddDays(-1);

            return _context.Ventas
                .AsNoTracking()
                .Where(v => v.FechaRegistro.Date >= inicioMesPasado.Date && v.FechaRegistro.Date <= finMesPasado.Date)
                .Sum(v => (decimal?)v.MontoTotal) ?? 0;
        }
        public decimal ObtenerTotalInversionMesAnterior()
        {
            // Calculamos el rango del mes pasado
            DateTime inicioMesPasado = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime finMesPasado = inicioMesPasado.AddMonths(1).AddDays(-1);

            return _context.Compras
                .AsNoTracking()
                .Where(c => c.FechaRegistro.Date >= inicioMesPasado.Date && c.FechaRegistro.Date <= finMesPasado.Date)
                .Sum(c => (decimal?)c.MontoTotal) ?? 0;
        }
    }
}