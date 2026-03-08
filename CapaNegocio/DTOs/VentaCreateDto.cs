using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class VentaCreateDto
    {
        public int UsuarioId { get; set; }
        public int ClienteId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleVentaCreateDto> Detalles { get; set; } = new List<DetalleVentaCreateDto>();
    }
}
