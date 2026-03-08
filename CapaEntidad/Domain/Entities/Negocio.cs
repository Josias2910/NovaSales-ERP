using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidad.Domain.Entities
{
    public class Negocio
    {
        public int Id { get; set; }
        public int PuntoVenta { get; set; }
        public string Nombre { get; set; } = null!; 
        public string CUIT { get; set; } = null!; 
        public string Direccion { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? SitioWeb { get; set; }
        public string? Lema { get; set; }
        public string? Licencia { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public byte[]? Logo { get; set; }
    }
}
