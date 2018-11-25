using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models.Domain;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbSet<Role> _roles;
        private readonly ApplicationDbContext _context;

        public  RoleRepository (ApplicationDbContext context)
        {
            _context = context;
            _roles = context.Roles;
        }

        public Role GetByName(string name)
        {
            return _roles.FirstOrDefault(r => r.Name == name);
        }
    }
}
