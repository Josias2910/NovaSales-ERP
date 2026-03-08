using System;
using System.Collections.Generic;
using System.Text;
using CapaEntidad.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace SistemaVentas.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Negocio> Negocios { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("CadenaSQL");
                options.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Permiso>()
                .HasOne(p => p.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(p => p.RolId) 
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Compra>()
               .HasOne(c => c.Usuario)
               .WithMany(u => u.Compras)
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Compra>()
               .HasOne(c => c.Proveedor)
               .WithMany(p => p.Compras)
               .HasForeignKey(c => c.ProveedorId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Compra)
                .WithMany(c => c.DetallesCompra)
                .HasForeignKey(dc => dc.CompraId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Producto)
                .WithMany(p => p.DetallesCompra)
                .HasForeignKey(dc => dc.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany(v => v.DetallesVenta)
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.DetallesVenta)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Rol>().HasData
            (
                new Rol
                {
                    Id = 1,
                    Descripcion = "Administrador",
                    FechaRegistro = new DateTime(2026, 1, 1)
                }
            );
            modelBuilder.Entity<Rol>().HasData
            (
                new Rol
                {
                    Id = 2,
                    Descripcion = "Empleado",
                    FechaRegistro = new DateTime(2026, 1, 1)
                }
            );
            modelBuilder.Entity<Compra>().Property(c => c.MontoTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DetalleCompra>().Property(dc => dc.PrecioCompra).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DetalleCompra>().Property(dc => dc.PrecioVenta).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DetalleCompra>().Property(dc => dc.MontoTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Venta>().Property(v => v.MontoPago).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Venta>().Property(v => v.MontoCambio).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Venta>().Property(v => v.MontoTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DetalleVenta>().Property(dv => dv.PrecioVenta).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DetalleVenta>().Property(dv => dv.SubTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Producto>().Property(p => p.PrecioCompra).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Producto>().Property(p => p.PrecioVenta).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Permiso>().HasData
            (
                new Permiso { Id = 1, RolId = 1, NombreMenu = "menuUsuarios", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 2, RolId = 1, NombreMenu = "menuMantenedor", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 3, RolId = 1, NombreMenu = "menuVentas", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 4, RolId = 1, NombreMenu = "menuCompras", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 5, RolId = 1, NombreMenu = "menuClientes", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 6, RolId = 1, NombreMenu = "menuProveedores", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 7, RolId = 1, NombreMenu = "menuReportes", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 8, RolId = 1, NombreMenu = "menuAcercaDe", FechaRegistro = new DateTime(2026, 2, 17) }
            );
            modelBuilder.Entity<Permiso>().HasData
            (
                new Permiso { Id = 9, RolId = 2, NombreMenu = "menuVentas", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 10, RolId = 2, NombreMenu = "menuCompras", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 11, RolId = 2, NombreMenu = "menuClientes", FechaRegistro = new DateTime(2026, 2, 17) },
                new Permiso { Id = 12, RolId = 2, NombreMenu = "menuProveedores", FechaRegistro = new DateTime(2026, 2, 17)},
                new Permiso { Id = 13, RolId = 2, NombreMenu = "menuAcercaDe", FechaRegistro = new DateTime(2026, 2, 17) }
            );
        }
    }
}
