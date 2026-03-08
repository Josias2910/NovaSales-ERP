using System.Text;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CapaNegocio.DTOs;

namespace CapaNegocio.Services
{
    public class CategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public int Registar(CategoriaCreateDto dto)
        {
            if (_context.Categorias.Any(c => c.Descripcion == dto.Descripcion))
            {
                throw new Exception("La categoría ya existe.");
            }

            var categoria = new Categoria
            {
                Descripcion = dto.Descripcion,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria.Id;
        }

        public List<CategoriaListadoDto> ListarCategorias(int? filtro = 1)
        {
            var query = _context.Categorias.AsQueryable();

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
            return query.Select(c => new CategoriaListadoDto
            {
                Id = c.Id,
                Descripcion = c.Descripcion,
                Estado = c.Estado ? "Activo" : "Inactivo",
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy")
            }).ToList();
        }

        public List<CategoriaListadoDto> Buscar(string campo, string texto, int? filtro = 1)
        {
            var query = _context.Categorias.AsQueryable();

            switch (filtro)
            {
                case 1:
                    query = query.Where(c => c.Estado == true);
                    break;
                case 2:
                    query = query.Where(c => c.Estado == false);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(texto))
            {
                switch (campo)
                {
                    case "Descripcion":
                        query = query.Where(c => c.Descripcion.Contains(texto));
                        break;
                }
            }

            return query.Select(c => new CategoriaListadoDto
            {
                Id = c.Id,
                Descripcion = c.Descripcion,
                Estado = c.Estado ? "Activo" : "Inactivo",
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy")
            }).ToList();
        }

        public (int total, int activos, int inactivos) ObtenerResumenEstadistico()
        {
            var total = _context.Categorias.AsNoTracking().Count();
            var activos = _context.Categorias.AsNoTracking().Count(c => c.Estado == true);
            var inactivos = total - activos;

            return (total, activos, inactivos);
        }

        public bool Editar(CategoriaUpdateDto dto)
        {
            var categoria = _context.Categorias.Find(dto.Id);
            if (categoria == null) 
                throw new Exception("Categoría no encontrada.");

            bool existeDuplicado = _context.Categorias.Any(c => c.Descripcion == dto.Descripcion && c.Id != dto.Id);
            if (existeDuplicado)
            {
                throw new Exception("Otra categoría con la misma descripción ya existe.");
            }

            categoria.Descripcion = dto.Descripcion;

            return _context.SaveChanges() > 0;
        }

        public bool CambiarEstado(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null) 
                throw new Exception("Categoría no encontrada.");

            bool tieneProductos = _context.Productos.Any(p => p.CategoriaId == id);
            if (tieneProductos)
            {
                throw new Exception("No se puede desactivar la categoría porque tiene productos asociados.");
            }
            categoria.Estado = !categoria.Estado;
            return _context.SaveChanges() > 0;
        }
    }
}
