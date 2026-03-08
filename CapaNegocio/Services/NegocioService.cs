using CapaNegocio.DTOs;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CapaNegocio.Services
{
    public class NegocioService
    {
        private readonly AppDbContext _context;

        public NegocioService(AppDbContext context)
        {
            _context = context;
        }

        public bool GuardarCambios(NegocioCreateDto dto)
        {
            var todosLosNegocios = _context.Negocios.ToList();

            var negocioPrincipal = todosLosNegocios.FirstOrDefault();

            if (negocioPrincipal == null)
            {
                negocioPrincipal = new CapaEntidad.Domain.Entities.Negocio();
                _context.Negocios.Add(negocioPrincipal);
            }
            else if (todosLosNegocios.Count > 1)
            {
                var duplicados = todosLosNegocios.Skip(1); 
                _context.Negocios.RemoveRange(duplicados);
            }

            negocioPrincipal.Nombre = dto.Nombre;
            negocioPrincipal.CUIT = dto.CUIT;
            negocioPrincipal.Direccion = dto.Direccion;
            negocioPrincipal.PuntoVenta = dto.PuntoVenta;
            negocioPrincipal.Telefono = dto.Telefono;
            negocioPrincipal.Correo = dto.Correo;
            negocioPrincipal.SitioWeb = dto.SitioWeb;
            negocioPrincipal.Lema = dto.Lema;
            negocioPrincipal.Logo = dto.Logo;

            return _context.SaveChanges() > 0;
        }

        public bool EliminarLogo()
        {
            var negocio = _context.Negocios.FirstOrDefault();
            if (negocio != null)
            {
                negocio.Logo = null;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public NegocioCreateDto ListarDatos()
        {
            var n = _context.Negocios.FirstOrDefault();
            if (n == null) return new NegocioCreateDto();

            return new NegocioCreateDto
            {
                Nombre = n.Nombre,
                CUIT = n.CUIT,
                Direccion = n.Direccion,
                Telefono = n.Telefono,
                Correo = n.Correo,
                SitioWeb = n.SitioWeb,
                Lema = n.Lema,
                Logo = n.Logo,
                PuntoVenta = n.PuntoVenta
            };
        }
        public bool ActivarSoftware(string llaveCompleta, DateTime fechaVence)
        {
            try
            {
                var negocio = _context.Negocios.FirstOrDefault();

                if (negocio != null)
                {
                    negocio.Licencia = llaveCompleta;
                    negocio.FechaVencimiento = fechaVence;

                    _context.Entry(negocio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    return _context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
