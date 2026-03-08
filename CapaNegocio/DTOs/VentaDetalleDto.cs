using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class VentaDetalleDto
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaRegistro { get; set; }
        public string UsuarioNombre { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }
        public List<DetalleVentaCreateDto> Detalles { get; set; }
    }
}
