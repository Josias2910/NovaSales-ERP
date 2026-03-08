using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class ReporteCompraDto
    {
        public int IdCompra { get; set; }
        public string FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string UsuarioRegistro { get; set; }
        public decimal MontoTotal { get; set; }

        // Datos del Proveedor
        public string DocumentoProveedor { get; set; }
        public string RazonSocial { get; set; }

        // Datos del Detalle del Producto (Para que la grilla sea completa)
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
