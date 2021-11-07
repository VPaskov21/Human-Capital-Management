using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IRoleRepository
    {
        Role GetRoleById(int roleId);
        Role GetRoleByName(string roleName);
        List<Role> GetRoles();
    }
}