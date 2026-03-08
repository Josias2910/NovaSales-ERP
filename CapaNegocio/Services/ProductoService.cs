using System.Text;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CapaNegocio.DTOs;

namespace CapaNegocio.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public void Registar(ProductoCreateDto dto)
        {
            if (_context.Productos.Any(c => c.Codigo.Trim().ToUpper() == dto.Codigo.Trim().ToUpper()))
            {
                throw new Exception("El código de producto ya se encuentra registrado.");
            }

            var producto = new Producto
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                Stock = 0,
                PrecioCompra = 0,
                PrecioVenta = 0,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            _context.Productos.Add(producto);
            _context.SaveChanges();
        }
        public ProductoListadoDto GetByCodigo(string codigo)
        {
            var query = _context.Productos.Include(p => p.Categoria).Where(p => p.Codigo == codigo).AsNoTracking().AsQueryable();
            return query.Select(p => new ProductoListadoDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria.Descripcion,
                Descripcion = p.Descripcion,
                Stock = p.Stock,
                Imagen = p.Imagen,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Estado = p.Estado ? "Activo" : "Inactivo",
            }).FirstOrDefault();
        }
        public List<ProductoListadoDto> ListarProductos(int? filtro = 1, bool minimal = false)
        {
            var query = _context.Productos.Include(p => p.Categoria).AsNoTracking().AsQueryable();

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
            return query.Select(p => new ProductoListadoDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria.Descripcion,
                // Si minimal es true, mandamos null o vacío a lo que no necesitamos
                Descripcion = minimal ? "" : p.Descripcion,
                Stock = minimal ? 0 : p.Stock,
                PrecioCompra = minimal ? 0 : p.PrecioCompra,
                PrecioVenta = minimal ? 0 : p.PrecioVenta,
                Estado = minimal ? "" : p.Estado ? "Activo" : "Inactivo",
            }).ToList();
        }

        public List<ProductoListadoDto> Buscar(string campo, string texto, int? filtro = 1)
        {
            var query = _context.Productos.Include(p => p.Categoria).AsNoTracking().AsQueryable();

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
                    case "Descripcion":
                        query = query.Where(p => p.Descripcion.Contains(texto));
                        break;
                    case "Nombre":
                        query = query.Where(p => p.Nombre.Contains(texto));
                        break;
                    case "Categoria":
                        query = query.Where(p => p.Categoria.Descripcion.ToLower().Contains(texto.ToLower()));
                        break;
                    case "Codigo":
                        query = query.Where(p => p.Codigo.Contains(texto));
                        break;
                }
            }

            return query.Select(p => new ProductoListadoDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Categoria = p.Categoria.Descripcion,
                Stock = p.Stock,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Estado = p.Estado ? "Activo" : "Inactivo",
            }).ToList();
        }

        public (int total, int activos, int inactivos, int criticos, decimal capital) ObtenerResumenEstadistico()
        {
            var productos = _context.Productos.AsNoTracking().ToList();
            var total = _context.Productos.AsNoTracking().Count();
            var activos = _context.Productos.AsNoTracking().Count(c => c.Estado == true);
            var inactivos = total - activos;
            var criticos = productos.Count(p => p.Stock < 10 && p.Estado == true);
            var capital = productos.Where(p => p.Estado == true).Sum(p => p.Stock * p.PrecioVenta);

            return (total, activos, inactivos, criticos, capital);
        }

        public bool Editar(ProductoUpdateDto dto)
        {
            var producto = _context.Productos.Find(dto.Id);
            if (producto == null)
                throw new Exception("Producto no encontrado.");

            if (string.IsNullOrWhiteSpace(dto.Codigo)) throw new Exception("El código es obligatorio.");

            bool codigoDuplicado = _context.Productos.Any(p => p.Codigo == dto.Codigo && p.Id != dto.Id);
            if (codigoDuplicado)
            {
                throw new Exception("El código de producto ya está siendo usado por otro registro.");
            }

            producto.Nombre = dto.Nombre;
            producto.Codigo = dto.Codigo;
            producto.Descripcion = dto.Descripcion;
            producto.CategoriaId = dto.CategoriaId;

            return _context.SaveChanges() > 0;
        }

        public bool CambiarEstado(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
                throw new Exception("Producto no encontrado.");

            producto.Estado = !producto.Estado;
            return _context.SaveChanges() > 0;
        }

        public bool ActualizarImagen(int id, byte[] imagenBytes, out string mensaje)
        {
            mensaje = "";
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto == null)
                {
                    mensaje = "Producto no encontrado.";
                    return false;
                }
                producto.Imagen = imagenBytes;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool EliminarImagen(int idProducto, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                var producto = _context.Productos.Find(idProducto);
                if (producto != null)
                {
                    producto.Imagen = null; // Seteamos a null en la BD
                    _context.SaveChanges();
                    return true;
                }
                mensaje = "Producto no encontrado.";
                return false;
            }
            catch (Exception ex)
            {
                mensaje = "Error al eliminar imagen: " + ex.Message;
                return false;
            }
        }
        public byte[] ObtenerImagen(int idProducto)
        {
            // Buscamos solo la propiedad Imagen para no sobrecargar la memoria
            return _context.Productos
                .Where(p => p.Id == idProducto)
                .Select(p => p.Imagen)
                .FirstOrDefault();
        }
    }
}