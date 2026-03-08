using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class CompraDetalleDto
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaRegistro { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoTotal { get; set; }
        public string UsuarioNombre { get; set; }
        public string DocumentoProveedor { get; set; }
        public string RazonSocial { get; set; }
        public List<DetalleCompraCreateDto> Detalles { get; set; }
    }
}
