using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class DetalleVentaCreateDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal SubTotal { get; set; }
    }
}
