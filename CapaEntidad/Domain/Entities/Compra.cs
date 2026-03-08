using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
