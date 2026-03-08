using System;
using System.Collections.Generic;
using System.Text;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Data;
using CapaNegocio.DTOs;

namespace CapaNegocio.Services
{
    public class RolService
    {
        private readonly AppDbContext _context;

        public RolService(AppDbContext context)
        {
            _context = context;
        }

        public List<Rol> ObtenerRoles()
        {
            return _context.Roles.ToList();
        }
        public List<RolComboDto> ObtenerRolesCombo()
        {
            return _context.Roles.Select(r => new RolComboDto { Id = r.Id, Descripcion = r.Descripcion }).ToList();
        }
    }
}
