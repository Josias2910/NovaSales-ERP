using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }    
        public decimal MontoTotal { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
