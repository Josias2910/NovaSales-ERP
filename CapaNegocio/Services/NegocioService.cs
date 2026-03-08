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

        // Cambiamos Registrar por un método que guarde o actualice (Upsert)
        public bool GuardarCambios(NegocioCreateDto dto)
        {
            // Traemos TODOS los registros de negocio que existan
            var todosLosNegocios = _context.Negocios.ToList();

            // Tomamos el primero como el "oficial"
            var negocioPrincipal = todosLosNegocios.FirstOrDefault();

            if (negocioPrincipal == null)
            {
                negocioPrincipal = new CapaEntidad.Domain.Entities.Negocio();
                _context.Negocios.Add(negocioPrincipal);
            }
            else if (todosLosNegocios.Count > 1)
            {
                // --- LIMPIEZA: Si hay duplicados, borramos los demás ---
                var duplicados = todosLosNegocios.Skip(1); // Todos menos el primero
                _context.Negocios.RemoveRange(duplicados);
            }

            // Mapeamos los datos (Tu lógica actual que está perfecta)
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
                negocio.Logo = null; // Así se elimina realmente en EF Core
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        // Necesitarás este método para cargar el formulario al iniciar
        public NegocioCreateDto ListarDatos()
        {
            var n = _context.Negocios.FirstOrDefault();
            if (n == null) return new NegocioCreateDto(); // Devuelve uno vacío si no hay nada

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
                // Buscamos el registro de configuración
                var negocio = _context.Negocios.FirstOrDefault();

                if (negocio != null)
                {
                    // Guardamos los dos datos clave
                    negocio.Licencia = llaveCompleta;       // Ej: "ABC123XYZ-20261231"
                    negocio.FechaVencimiento = fechaVence;  // El objeto DateTime real

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
