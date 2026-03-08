using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class ReporteStockDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; } // Lo traeremos de Categoria.Descripcion
        public int Stock { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        // Dato de valor agregado para el ERP: cuánto dinero hay "parado" en este producto
        public decimal MontoTotalStock => Stock * PrecioCompra;

        // Útil para alertas visuales en la grilla
        public bool EsStockBajo => Stock <= 10;

        // Lo que ganarías si vendes TODAS las unidades de este producto
        public decimal GananciaPotencialItem => (PrecioVenta - PrecioCompra) * Stock;
    }
}
