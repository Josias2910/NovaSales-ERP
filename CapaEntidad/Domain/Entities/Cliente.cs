using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
