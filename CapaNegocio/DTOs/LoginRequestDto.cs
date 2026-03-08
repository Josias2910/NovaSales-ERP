using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class LoginRequestDto
    {
        public string Documento { get; set; }
        public string Clave { get; set; }
    }
}
