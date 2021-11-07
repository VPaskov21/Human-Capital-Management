using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Department> GetDepartments() => context.Departments.Include(n => n.Roles)
                                                                        .Include(n => n.Users).ToList();

        public Department GetDepartmentByName(string name) => GetDepartments().FirstOrDefault(n => n.DepartmentName.Equals(name));

        public Department GetDepartmentById(int departmentId) => GetDepartments().Where(n => n.Id == departmentId).FirstOrDefault();
    }
}
