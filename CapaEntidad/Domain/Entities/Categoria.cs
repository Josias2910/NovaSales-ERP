using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Estado { get; set; }
    }
}
