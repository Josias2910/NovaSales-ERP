using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class CompraCreateDto
    {
        public int UsuarioId { get; set; }
        public int ProveedorId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleCompraCreateDto> Detalles { get; set; } = new List<DetalleCompraCreateDto>();
    }
}
