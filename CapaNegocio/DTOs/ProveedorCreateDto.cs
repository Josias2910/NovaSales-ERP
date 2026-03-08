using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class ProveedorCreateDto
    {
        public string Documento { get; set; }
        public string RazonSocial { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
}
