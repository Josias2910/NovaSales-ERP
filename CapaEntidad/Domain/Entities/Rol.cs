using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
