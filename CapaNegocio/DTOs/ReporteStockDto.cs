using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class ReporteStockDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; } 
        public int Stock { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal MontoTotalStock => Stock * PrecioCompra;
        public bool EsStockBajo => Stock <= 10;
        public decimal GananciaPotencialItem => (PrecioVenta - PrecioCompra) * Stock;
    }
}
