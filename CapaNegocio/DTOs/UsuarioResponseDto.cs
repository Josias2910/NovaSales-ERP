using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string Rol { get; set; }
        public List<string> Permisos { get; set; }
    }
}
