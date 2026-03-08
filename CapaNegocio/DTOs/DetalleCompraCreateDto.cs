using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class DetalleCompraCreateDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string Codigo { get; set; }
        public string ProductoNombre { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
