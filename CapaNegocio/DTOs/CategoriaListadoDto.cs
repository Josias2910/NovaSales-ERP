using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class CategoriaListadoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string FechaRegistro { get; set; }
    }
}
