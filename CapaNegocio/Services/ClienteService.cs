using CapaNegocio.DTOs;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public bool Registrar(ClienteCreateDto dto)
        {
            bool existe = _context.Clientes.Any(c => c.Documento == dto.Documento);
            if (existe)
            {
                throw new Exception("El cliente ya se encuentra registrado.");
            }

            var registrarCliente = new Cliente
            {
                Documento = dto.Documento,
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Estado = true
            };
            _context.Clientes.Add(registrarCliente);
            return _context.SaveChanges() > 0;
        }

        public bool Editar(ClienteUpdateDto dto)
        {
            var clientes = _context.Clientes.Find(dto.Id);
            if (clientes == null)
            {
                throw new Exception("Cliente no encontrado.");
            }
            bool documentoDuplicado = _context.Clientes.Any(c => c.Documento == dto.Documento && c.Id != dto.Id);
            if (documentoDuplicado) throw new Exception("El documento ya pertenece a otro cliente.");

            clientes.Documento = dto.Documento;
            clientes.NombreCompleto = dto.NombreCompleto;
            clientes.Correo = dto.Correo;
            clientes.Telefono = dto.Telefono;

            return _context.SaveChanges() > 0;
        }

        public List<ClienteListadoDto> Buscar(string campo, string texto, int? filtro = 1)
        {
            var query = _context.Clientes.AsNoTracking().AsQueryable();

            switch (filtro)
            {
                case 1: // Activos
                    query = query.Where(c => c.Estado == true);
                    break;
                case 2: // Inactivos
                    query = query.Where(c => c.Estado == false);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(texto))
            {
                switch (campo)
                {
                    case "Documento":
                        query = query.Where(c => c.Documento.Contains(texto));
                        break;
                    case "Nombre":
                        query = query.Where(c => c.NombreCompleto.Contains(texto));
                        break;
                    case "Correo":
                        query = query.Where(c => c.Correo.Contains(texto));
                        break;
                    case "Telefono":
                        query = query.Where(c => c.Telefono.Contains(texto));
                        break;
                }
            }

            return query.Select(c => new ClienteListadoDto
            {
                Id = c.Id,
                Documento = c.Documento,
                NombreCompleto = c.NombreCompleto,
                Correo = c.Correo,
                Telefono = c.Telefono,
                Estado = c.Estado ? "Activo" : "Inactivo",
            }).ToList();
        }
        public List<ClienteListadoDto> ListarClientes(int? filtro = 1, bool minimal = false)
        {
            var query = _context.Clientes.AsNoTracking().AsQueryable();

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

            return query.Select(c => new ClienteListadoDto
            {
                Id = c.Id,
                Documento = c.Documento,
                NombreCompleto = c.NombreCompleto,
                Correo = minimal ? "" : c.Correo,
                Telefono = minimal ? "" : c.Telefono,
                Estado = minimal ? "" : (c.Estado ? "Activo" : "Inactivo")
            }).ToList();
        }

        public bool CambiarEstado(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) throw new Exception("Cliente no encontrado.");

            cliente.Estado = !cliente.Estado;

            return _context.SaveChanges() > 0;
        }

        public (int total, int activos, int inactivos) ObtenerResumenEstadistico()
        {
            var clientes = _context.Clientes.AsNoTracking().ToList();
            int total = clientes.Count;
            int activos = clientes.Count(c => c.Estado);
            int inactivos = total - activos;

            return (total, activos, inactivos);
        }
    }
}
