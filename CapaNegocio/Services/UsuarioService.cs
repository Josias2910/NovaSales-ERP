using CapaNegocio.DTOs;
using CapaNegocio.Utilidades;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CapaNegocio.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }
        public List<Usuario> Listar()
        {
            return _context.Usuarios.ToList();
        }
        public UsuarioResponseDto Login(LoginRequestDto request)
        {
            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                .ThenInclude(p => p.Permisos)
                .FirstOrDefault(u => u.Documento == request.Documento && u.Estado == true);

            if (usuario == null)
            {
                return null;
            }

            bool passwordValida = SeguridadPassword.ValidarPassword(request.Clave, usuario.Clave);

            if (!passwordValida)
            {
                return null; // Contraseña incorrecta
            }
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.NombreCompleto,
                Documento = usuario.Documento,
                Rol = usuario.Rol.Descripcion,
                Permisos = usuario.Rol.Permisos.Select(p => p.NombreMenu).ToList()
            };
        }
        public List<UsuarioListadoDto> ListarUsuarios(int? filtro = 1)
        {
            var query = _context.Usuarios.Include(u => u.Rol).AsQueryable();

            switch (filtro)
            {
                case 1: // Activos
                    query = query.Where(u => u.Estado == true);
                    break;

                case 2: // Inactivos
                    query = query.Where(u => u.Estado == false);
                    break;

                case 3: // Todos
                default:
                    break;
            }

            return query.Select(u => new UsuarioListadoDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                Documento = u.Documento,
                Rol = u.Rol.Descripcion,
                Estado = u.Estado ? "Activo" : "Inactivo"
            }).ToList();
        }
        public void Guardar(UsuarioCreateDto guardar)
        {
            if (string.IsNullOrWhiteSpace(guardar.NombreCompleto))
                throw new Exception("Nombre obligatorio");
            if (_context.Usuarios.Any(u => u.Documento == guardar.Documento))
                throw new Exception("Ya existe un usuario con ese documento");
            if (_context.Usuarios.Any(u => u.Correo == guardar.Correo))
                throw new Exception("Ya existe un usuario con ese correo");
            if (string.IsNullOrWhiteSpace(guardar.Clave))
                throw new Exception("Clave obligatoria");
            if (guardar.Clave != guardar.ConfirmarClave)
                throw new Exception("Las claves no coinciden");

            if (guardar.Clave != guardar.ConfirmarClave)
                throw new Exception("Las claves no coinciden");

            var usuario = new Usuario
            {
                NombreCompleto = guardar.NombreCompleto,
                Correo = guardar.Correo,
                Documento = guardar.Documento,
                Clave = SeguridadPassword.HashPassword(guardar.Clave),
                RolId = guardar.RolId,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
        public List<UsuarioListadoDto> Buscar(string campo, string texto)
        {
            var query = _context.Usuarios.Include(u => u.Rol).AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                switch (campo)
                {
                    case "Documento":
                        query = query.Where(u => u.Documento.Contains(texto));
                        break;
                    case "Nombre":
                        query = query.Where(u => u.NombreCompleto.Contains(texto));
                        break;
                    case "Correo":
                        query = query.Where(u => u.Correo.Contains(texto));
                        break;
                }
            }

            return query.Select(u => new UsuarioListadoDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                Documento = u.Documento,
                Rol = u.Rol.Descripcion,
                Estado = u.Estado ? "Activo" : "Inactivo"
            }).ToList();
        }
        public bool CambiarEstado(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            usuario.Estado = !usuario.Estado;
            return _context.SaveChanges() > 0;
        }
        public bool Editar(UsuarioUpdateDto editar)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == editar.Id);
            if (usuario == null) return false;

            usuario.NombreCompleto = editar.NombreCompleto;
            usuario.Correo = editar.Correo;
            usuario.Documento = editar.Documento;
            usuario.RolId = editar.RolId;

            if (!string.IsNullOrWhiteSpace(editar.Clave))
            {
                usuario.Clave = SeguridadPassword.HashPassword(editar.Clave);
            }

            _context.SaveChanges();
            return true;
        }
        public (int totales, int activos, int inactivos) ObtenerResumenEstadisticas()
        {
            var todos = _context.Usuarios.ToList();
            int totales = todos.Count;
            int activos = todos.Count(u => u.Estado == true);
            int inactivos = todos.Count(u => u.Estado == false);

            return (totales, activos, inactivos);
        }
    }
}
