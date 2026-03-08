using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CapaNegocio.DTOs;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Domain.Entities;
using System.Data;

namespace CapaNegocio.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public void Registrar(ProveedorCreateDto dto)
        {
            var proveedores = _context.Proveedores.Any(p => p.Documento == dto.Documento);
            if(proveedores)
            {
                throw new Exception("El proveedor ya existe");
            }

            var proveedor = new Proveedor
            {
                Documento = dto.Documento,
                RazonSocial = dto.RazonSocial,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Estado = true
            };
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
        }

        public List<ProveedorListadoDto> ListarProveedores(int? filtro = 1, bool minimal = false)
        {
            var query = _context.Proveedores.AsNoTracking().AsQueryable();

            switch (filtro)
            {
                case 1: // Activos
                    query = query.Where(c => c.Estado == true);
                    break;
                case 2: // Inactivos
                    query = query.Where(c => c.Estado == false);
                    break;
                case 3: // Todos
                default:
                    break;
            }
            return query.Select(p => new ProveedorListadoDto
            {
                Id = p.Id,
                Documento = p.Documento,
                RazonSocial = p.RazonSocial,
                // Si minimal es true, mandamos null o vacío a lo que no necesitamos
                Telefono = minimal ? "" : p.Telefono,
                Correo = minimal ? "" : p.Correo,
                Estado = minimal ? "" : (p.Estado ? "Activo" : "Inactivo")
            }).ToList();
        }

        public bool Editar(ProveedorUpdateDto dto)
        {
            var proveedor = _context.Proveedores.Find(dto.Id);
            if (proveedor == null)
            {
                throw new Exception("Proveedor no encotrado.");
            }
            
            bool existe = _context.Proveedores.Any(p => p.Documento == dto.Documento && p.Id != dto.Id);
            if (existe)
            {
                throw new Exception("El documento ya existe para otro proveedor.");
            }

            proveedor.Documento = dto.Documento;
            proveedor.RazonSocial = dto.RazonSocial;
            proveedor.Telefono = dto.Telefono;
            proveedor.Correo = dto.Correo;

            return _context.SaveChanges() > 0;
        }

        public List<ProveedorListadoDto> Buscar(string texto, string campo, int? filtro = 1)
        {
            var query = _context.Proveedores.AsNoTracking().AsQueryable();
            switch (filtro)
            {
                case 1:
                    query = query.Where(p => p.Estado == true);
                    break;
                case 2:
                    query = query.Where(p => p.Estado == false);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(texto))
            {
                switch (campo)
                {
                    case "Documento":
                        query = query.Where(p => p.Documento.ToLower().Contains(texto.ToLower()));
                        break;
                    case "Razon Social":
                        query = query.Where(p => p.RazonSocial.ToLower().Contains(texto.ToLower()));
                        break;
                    case "Telefono":
                        query = query.Where(p => p.Telefono.Contains(texto));
                        break;
                    case "Correo":
                        query = query.Where(p => p.Correo.ToLower().Contains(texto.ToLower()));
                        break;
                }
            }

            return query.Select(p => new ProveedorListadoDto
            {
                Id = p.Id,
                Documento = p.Documento,
                RazonSocial = p.RazonSocial,
                Telefono = p.Telefono,
                Correo = p.Correo,
                Estado = p.Estado ? "Activo" : "Inactivo"
            }).ToList();
        }

        public bool CambiarEstado(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null)
            {
                throw new Exception("Proveedor no encontrado.");
            }
            proveedor.Estado = !proveedor.Estado;
            return _context.SaveChanges() > 0;
        }

        public (int total, int activos, int inactivos) ObtenerResumenEstadistico()
        {
            var proveedor = _context.Proveedores.AsNoTracking().ToList();
            var total = _context.Proveedores.AsNoTracking().Count();
            var activos = _context.Proveedores.AsNoTracking().Count(c => c.Estado == true);
            var inactivos = total - activos;

            return (total, activos, inactivos);
        }
    }
}
