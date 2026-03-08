using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class VentaListadoDto
    {
        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreUsuario { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
