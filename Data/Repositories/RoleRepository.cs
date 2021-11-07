using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext context;

        public RoleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Role> GetRoles() => context.Roles.Include(n => n.Department).ToList();

        public Role GetRoleById(int roleId) => GetRoles().SingleOrDefault(n => n.Id == roleId);

        public Role GetRoleByName(string roleName) => GetRoles().SingleOrDefault(n => n.RoleName.ToLower().Equals(roleName.ToLower()));
    }
}
