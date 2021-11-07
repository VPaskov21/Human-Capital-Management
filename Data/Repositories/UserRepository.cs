using HCMApp.Data.Models;
using HCMApp.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<User> GetUsers() => context.Users.Include(n => n.Department)
                                                      .Include(n => n.Salary)
                                                      .Include(n => n.Role)
                                                      .Include(n => n.UserType)
                                                      .Include(n => n.Address)
                                                       .ThenInclude(n => n.City)
                                                      .Include(n => n.Address)
                                                       .ThenInclude(n => n.City)
                                                       .ThenInclude(n => n.PostalCode)
                                                      .Include(n => n.Address)
                                                       .ThenInclude(n => n.City)
                                                       .ThenInclude(n => n.Region)
                                                       .ThenInclude(n => n.Country)
                                                      .Include(n => n.Address)
                                                       .ThenInclude(n => n.City)
                                                      .Include(n => n.Leave_Days)
                                                      .Include(n => n.Absence_Requests)
                                                      .Include(n => n.SalaryHistories)
                                                      .ToList();

        public User GetUser(string username, string password) => context.Users.Include(n => n.UserType).FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));

        public User GetUserByUsername(string username) => GetUsers().Where(n => n.Username.Equals(username)).SingleOrDefault();

        public string GetUsersNamesByUsername(string username)
        {
            var user = GetUserByUsername(username);
            return user.FirstName + " " + user.LastName;
        }

        public int GetAverageEmployeeAge()
        {
            var age = 0;
            foreach(var person in context.Users)
            {
                var personAge = DateTime.UtcNow.Year - person.Birthday.Year;
                age += personAge;
            }

            return (int) age / context.Users.Count();
        }

        public int GetAverageEmployeeSalary()
        {
            var salary = 0;
            foreach(var person in context.Users.Include(n => n.Salary))
            {
                salary += person.Salary.netSalary;
            }

            return (int) salary / context.Users.Count();
        }

        public int GetEmployeeCount() => context.Users.Count();

        public List<ViewEmployeesVM> GetUsersVM()
        {
             return GetUsers().Select(employee => new ViewEmployeesVM()
            {
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Email = employee.Email,
                Role = employee.Role.RoleName,
                Department = employee.Department.DepartmentName,
                Salary = employee.Salary.grossSalary.ToString(),
                ImageSrc = employee.ImageSrc
            }).OrderBy(n => n.Department).ThenBy(n => n.Role).ToList();
        }

        public ViewEmployeesVM GetUserVMByEmail(string email)
        {
            var user = GetUsers().Where(n => n.Email.Equals(email))
                .Select(employee => new ViewEmployeesVM()
                {
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                    Address = employee.Address.StreetAddress,
                    Address_Additional = employee.Address.StreetAddressAdditional,
                    Gender = employee.Gender,
                    City = employee.Address.City.CityName,
                    Region = employee.Address.City.Region.RegionName,
                    Postal_Code = employee.Address.City.PostalCode.PostalCodeNumber.ToString(),
                    Birthday = employee.Birthday,
                    Start_Date = employee.Start_Date,
                    Role = employee.Role.RoleName,
                    Salary = employee.Salary.grossSalary.ToString().Replace(",","."),
                    Department = employee.Department.DepartmentName,
                    Country = employee.Address.City.Region.Country.CountryName,
                    ImageSrc = employee.ImageSrc,
                    Username = employee.Username
                }).FirstOrDefault();
            return user;
        }

        public User GetUserByEmail(string email) => GetUsers().Where(n => n.Email.ToLower().Equals(email.ToLower())).SingleOrDefault();

        public int GetLeftEmployeesCount() => context.LeftEmployees.Count();

        public List<LeftEmployee> GetLeftEmployees() => context.LeftEmployees
                                                                    .Include(n => n.Department)
                                                                    .Include(n => n.Role)
                                                                    .Include(n => n.Salary)
                                                                    .Include(n => n.Address)
                                                                       .ThenInclude(n => n.City)
                                                                      .Include(n => n.Address)
                                                                       .ThenInclude(n => n.City)
                                                                       .ThenInclude(n => n.PostalCode)
                                                                      .Include(n => n.Address)
                                                                       .ThenInclude(n => n.City)
                                                                       .ThenInclude(n => n.Region)
                                                                       .ThenInclude(n => n.Country)
                                                                      .Include(n => n.Address)
                                                                       .ThenInclude(n => n.City)
                                                                        .ToList();
    }
}
