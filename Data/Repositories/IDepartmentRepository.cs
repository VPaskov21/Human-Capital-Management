using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IDepartmentRepository
    {
        Department GetDepartmentById(int departmentId);
        Department GetDepartmentByName(string name);
        List<Department> GetDepartments();
    }
}