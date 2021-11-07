using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class RoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;
        private readonly ValidationService validationService;

        public RoleService(IRoleRepository roleRepository, AppDbContext context,
            IUserRepository userRepository,
            ValidationService validationService)
        {
            this.roleRepository = roleRepository;
            this.context = context;
            this.userRepository = userRepository;
            this.validationService = validationService;
        }

        public List<RoleVM> GetRolesForDepartment(int departmentId)
        {
            List<Role> roles = roleRepository.GetRoles();

            List<RoleVM> rolesInDepartment = context.Roles.Where(n => n.DepartmentId == departmentId)
                .Select(n => new RoleVM()
                {
                    Id = n.Id,
                    RoleName = n.RoleName
                }).ToList();

            return rolesInDepartment;
        }

        public RoleVM GetRoleForEmployee(string email)
        {
            var currentEmployeeRole = userRepository.GetUserVMByEmail(email).Role;
            return new RoleVM() { RoleName = currentEmployeeRole };
        }

        public List<Role> GetRoles() => roleRepository.GetRoles();

        public string CreateRole(string roleName, int departmentId, string minSalary, string maxSalary)
        {
            try
            {
                if(!validationService.ValidateRole(roleName, minSalary, maxSalary))
                {
                    return "ValidationFailed";
                }

                var role = GetRolesForDepartment(departmentId).Where(n => n.RoleName.ToLower().Equals(roleName.ToLower())).SingleOrDefault();

                if(role == null)
                {
                    context.Roles.Add(new Role()
                    {
                        RoleName = roleName,
                        MinimalSalary = Convert.ToDouble(minSalary, CultureInfo.InvariantCulture),
                        MaximumSalary = Convert.ToDouble(maxSalary, CultureInfo.InvariantCulture),
                        DepartmentId = departmentId
                    });

                    context.SaveChanges();
                } else
                {
                    return "RoleDuplicate";
                }

                return "Success";
            }
            catch
            {
                return "Exception";
            }
        }
    }
}
