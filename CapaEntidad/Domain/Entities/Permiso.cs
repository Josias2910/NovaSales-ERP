using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Permiso
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
        public string NombreMenu { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
