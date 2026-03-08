using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    namespace CapaNegocio.DTOs.Reportes
    {
        public class ReporteResumenDto
        {
            // --- SECCIÓN MONETARIA ---
            public decimal TotalVentas { get; set; }
            public decimal TotalCompras { get; set; }
            public decimal GananciaNeta => TotalVentas - TotalCompras;
            public decimal PromedioTicketVenta => CantidadVentas > 0 ? TotalVentas / CantidadVentas : 0;

            // --- SECCIÓN OPERATIVA ---
            public int CantidadVentas { get; set; }
            public int CantidadCompras { get; set; }
            public int CantidadClientes { get; set; }
            public int ProductosBajoStock { get; set; } // Cuántos productos están en alerta roja

            // --- SECCIÓN DE RENDIMIENTO ---
            public string ProductoMasVendido { get; set; }
            public int UnidadesProductoMasVendido { get; set; }
            public string ClienteFrecuente { get; set; } // El que más veces compró
            public string MetodoPagoPreferido { get; set; } // Efectivo, Débito, etc.

            // --- SECCIÓN DE FECHAS ---
            public string FechaInicio { get; set; }
            public string FechaFin { get; set; }

            public decimal PromedioVentaDiaria => TotalVentas / Math.Max((DateTime.Parse(FechaFin) - DateTime.Parse(FechaInicio)).Days + 1, 1);
            public string ProveedorPrincipal { get; set; }
            public decimal PorcentajeROI => TotalCompras > 0 ? (GananciaNeta / TotalCompras) * 100 : 0;
            public double ArticulosPorTicket { get; set; }
            public string DiaPicoVenta { get; set; } // Ejemplo: "15/02 ($5,000)"

            // --- VARIACIONES (PORCENTAJES) ---
            public decimal VariacionVentas { get; set; } // Ejemplo: 15.5 (significa +15.5%)
            public decimal VariacionCompras { get; set; }
            public decimal VariacionTicket { get; set; }
            public decimal VariacionCantidadVentas { get; set; }
            public List<decimal> VentasPorDia { get; set; } = new List<decimal>();
            public List<decimal> ComprasPorDia { get; set; } = new List<decimal>();
            public List<string> EtiquetasDias { get; set; } = new List<string>();
        }
    }
}
