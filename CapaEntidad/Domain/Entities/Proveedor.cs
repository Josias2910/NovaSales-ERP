using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.Domain.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }
        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public string Documento { get; set; }
        public string RazonSocial { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
