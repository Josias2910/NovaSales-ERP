using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class NegocioCreateDto
    {
        public string Nombre { get; set; }
        public string CUIT { get; set; }
        public string Direccion { get; set; }
        public int PuntoVenta { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string SitioWeb { get; set; }
        public string Lema { get; set; }
        public byte[] Logo { get; set; }
    }
}
