using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class ReporteVentaDto
    {
        public int IdVenta { get; set; }
        public string FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Usuario { get; set; } // NombreCompleto del Usuario
        public string DocumentoCliente { get; set; } // Documento del Cliente
        public string NombreCliente { get; set; } // NombreCompleto del Cliente
        public string MetodoPago { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal GananciaVenta { get; set; }
    }
}
