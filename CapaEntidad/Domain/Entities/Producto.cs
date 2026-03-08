using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Estado { get; set; }
        public byte[] Imagen { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
