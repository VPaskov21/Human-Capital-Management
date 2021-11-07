using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly ValidationService validationService;
        private readonly AppDbContext context;

        public DepartmentService(IDepartmentRepository departmentRepository,
            ValidationService validationService,
            AppDbContext context)
        {
            this.departmentRepository = departmentRepository;
            this.validationService = validationService;
            this.context = context;
        }

        public List<Department> GetDepartments() => departmentRepository.GetDepartments();

        public string CreateDepartment(string departmentName)
        {
            try
            {
                if(!validationService.ValidateDepartmentName(departmentName))
                {
                    return "ValidationFailed";
                }

                var department = GetDepartments().Where(n => n.DepartmentName.ToLower().Equals(departmentName.ToLower())).SingleOrDefault();

                if(department == null)
                {
                    context.Departments.Add(new Department()
                    {
                        DepartmentName = departmentName
                    });

                    context.SaveChanges();
                } 
                else
                {
                    return "DepartmentDuplicate";
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
