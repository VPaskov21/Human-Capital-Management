using HCMApp.Data.Models;
using HCMApp.Data.ViewModels;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IUserRepository
    {
        int GetAverageEmployeeAge();
        int GetAverageEmployeeSalary();
        int GetEmployeeCount();
        User GetUser(string username, string password);
        ViewEmployeesVM GetUserVMByEmail(string email);
        User GetUserByUsername(string username);
        List<ViewEmployeesVM> GetUsersVM();
        string GetUsersNamesByUsername(string username);
        List<User> GetUsers();
        User GetUserByEmail(string email);
        int GetLeftEmployeesCount();
        List<LeftEmployee> GetLeftEmployees();
    }
}