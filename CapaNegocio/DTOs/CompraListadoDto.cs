using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class CompraListadoDto
    {
        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string MetodoPago { get; set; }
        public string RazonSocialProveedor { get; set; } 
        public string NombreUsuario { get; set; }    
        public decimal MontoTotal { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
