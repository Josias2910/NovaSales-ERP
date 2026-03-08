using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class UsuarioCreateDto
    {
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Documento { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        public int RolId { get; set; }
    }
}
