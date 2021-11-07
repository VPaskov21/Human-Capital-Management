using Cyrillic.Convert;
using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class UserService
    {
        private AppDbContext context;
        private readonly IDepartmentRepository departmentsRepository;
        private readonly IUserRepository userRepository;
        private readonly SalaryService salaryService;
        private readonly AddressService addressService;
        private readonly ImageService imageService;
        private readonly IRoleRepository roleRepository;
        private readonly ValidationService validationService;

        public UserService(AppDbContext context, 
            IDepartmentRepository departmentsRepository, 
            IUserRepository userRepository, 
            SalaryService salaryService,
            AddressService addressService,
            ImageService imageService,
            IRoleRepository roleRepository,
            ValidationService validationService)
        {
            this.context = context;
            this.departmentsRepository = departmentsRepository;
            this.userRepository = userRepository;
            this.salaryService = salaryService;
            this.addressService = addressService;
            this.imageService = imageService;
            this.roleRepository = roleRepository;
            this.validationService = validationService;
        }

        public string AddUser(HttpContext httpContext, AddEmployeeVM employeeVM)
        {
            try
            {
                if(!validationService.ValidateEmployeeData(employeeVM))
                {
                    return "ValidationFailed";
                }

                var conversion = new Conversion();

                double grossSalary = Convert.ToDouble(employeeVM.Salary, CultureInfo.InvariantCulture);


                var _employee = new User()
                {
                    FirstName = employeeVM.FirstName,
                    MiddleName = employeeVM.MiddleName,
                    LastName = employeeVM.LastName,
                    Username = conversion.BulgarianCyrillicToLatin(employeeVM.FirstName[0].ToString() + employeeVM.MiddleName[0].ToString() + employeeVM.LastName),
                    Password = "random", //random password will be generated for every user
                    PhoneNumber = employeeVM.PhoneNumber,
                    Email = employeeVM.Email,
                    Gender = employeeVM.Gender,
                    Birthday = employeeVM.Birthday,
                    Start_Date = employeeVM.StartDate,
                    DepartmentId = employeeVM.Department,
                    Salary = salaryService.GetSalaryBySalaryGrossAmount(grossSalary),
                    RoleId = employeeVM.Role,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress(employeeVM.Address, employeeVM.Additional_Address, employeeVM.City, employeeVM.Region, employeeVM.Postal_Code, employeeVM.Country).Id
                };

                var users = userRepository.GetUsers();
                bool add = true;

                foreach(var user in users)
                {
                    if(user.FirstName.ToLower().Equals(_employee.FirstName.ToLower()) && 
                        user.MiddleName.ToLower().Equals(_employee.MiddleName.ToLower()) &&
                        user.LastName.ToLower().Equals(_employee.LastName.ToLower()) &&
                        user.Username.ToLower().Equals(_employee.Username.ToLower()) &&
                        user.Email.ToLower().Equals(_employee.Email.ToLower()))
                    {
                        add = false;
                        break;
                    }
                }

                if(add)
                {
                    context.Users.Add(_employee);
                    context.SaveChanges();

                    if (httpContext.Request.Form.Files.Count > 0)
                    {
                        imageService.SaveImage(httpContext, _employee.Username);
                    }

                    AddInitialLeaveDaysForUser(_employee.Username);
                    return "Success";
                } else
                {
                    return "DuplicateUser";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return "Exception";
            }
        }

        public List<ViewEmployeesVM> GetUsersWithoutCurrentUser(string username)
        {
            var employees = userRepository.GetUsersVM();
            var currentUser = userRepository.GetUserByUsername(username);
            foreach (var employee in employees)
            {
                if (employee.FirstName.Equals(currentUser.FirstName) &&
                    employee.MiddleName.Equals(currentUser.MiddleName) &&
                    employee.LastName.Equals(currentUser.LastName))
                {
                    employees.Remove(employee);
                    break;
                }
            }

            return employees;
        }

        public List<ViewEmployeesVM> GetEmployeesFromDepartmentWithoutCurrentUser(string username, string department)
        {
            var employees = GetUsersWithoutCurrentUser(username);
            var finalList = new List<ViewEmployeesVM>();
            foreach(var employee in employees)
            {
                if(employee.Department.ToLower().Equals(department.ToLower())) 
                {
                    finalList.Add(employee);
                }
            }

            return finalList;
        }

        public List<ViewEmployeesVM> GetEmployeesForSalaryRangeWithoutCurrentUser(string username, int minSalary, int maxSalary)
        {
            var employees = GetUsersWithoutCurrentUser(username);
            var finalList = new List<ViewEmployeesVM>();
            foreach (var employee in employees)
            {
                if (Convert.ToDouble(employee.Salary) > minSalary && Convert.ToDouble(employee.Salary) < maxSalary)
                {
                    finalList.Add(employee);
                }
            }

            return finalList;
        }

        public UserType GetCurrentUserType(string username)
        {
            var currentUser = userRepository.GetUserByUsername(username);

            return currentUser.UserType;
        }

        public string ChangeUserPassword(User user, PasswordVM passwordVM)
        {
            try
            {
                if(!validationService.ValidateEmployeePasswords(passwordVM))
                {
                    return "ValidationFailed";
                }
                if (user.Password.Equals(passwordVM.OldPassword))
                {
                    user.Password = passwordVM.NewPassword;
                }

                context.SaveChanges();

                return "Success";
            }
            catch
            {
                return "Exception";
            }
        }

        public int GetUserId(string username) => userRepository.GetUserByUsername(username).UserId;

        public void AddInitialLeaveDaysForUser(string username)
        {
            context.Leave_Days.Add(new Leave_Days()
            {
                Year = DateTime.UtcNow.Year,
                TotalLeaveDays = 20,
                AvailableLeaveDays = 20,
                UserId = GetUserId(username)
            });

            context.SaveChanges();
        }

        public int GetCurrentUserTotalLeaveDays(string username) => context.Users.Where(n => n.Username.Equals(username)).Single().Leave_Days.Where(n => n.Year.Equals(DateTime.UtcNow.Year)).Single().TotalLeaveDays;

        public int GetCurrentUserAvailableLeaveDays(string username) => context.Users.Where(n => n.Username.Equals(username)).Single().Leave_Days.Where(n => n.Year.Equals(DateTime.UtcNow.Year)).Single().AvailableLeaveDays;


        public string UpdateEmployee(HttpContext httpContext, EditEmployeeVM employeeVM)
        {
            var employee = userRepository.GetUserByEmail(employeeVM.Email);

            try
            {
                if (!validationService.ValidateEmployeeData(employeeVM))
                {
                    return "ValidationFailed";
                }

                if (employee.FirstName != employeeVM.FirstName)
                {
                    employee.FirstName = employeeVM.FirstName;
                }

                if (employee.MiddleName != employeeVM.MiddleName)
                {
                    employee.MiddleName = employeeVM.MiddleName;
                }

                if (employee.LastName != employeeVM.LastName)
                {
                    employee.LastName = employeeVM.LastName;
                }

                if (employee.PhoneNumber != employeeVM.PhoneNumber)
                {
                    employee.PhoneNumber = employeeVM.PhoneNumber;
                }

                if (httpContext.Request.Form.Files.Count > 0)
                {
                    imageService.SaveImage(httpContext, employee.Username);
                }

                if (employee.DepartmentId != Convert.ToInt16(employeeVM.Department))
                {
                    employee.DepartmentId = Convert.ToInt32(employeeVM.Department);
                }

                if (employee.RoleId != Convert.ToInt32(employeeVM.Role))
                {
                    employee.RoleId = Convert.ToInt32(employeeVM.Role);
                }

                if (employee.Salary.grossSalary != Convert.ToDouble(employeeVM.Salary, CultureInfo.InvariantCulture))
                {
                    employee.SalaryId = salaryService.GetSalaryBySalaryGrossAmount(Convert.ToDouble(employeeVM.Salary, CultureInfo.InvariantCulture)).Id;
                }

                if ((employee.Address.StreetAddress != employeeVM.Address) ||
                    (employee.Address.StreetAddressAdditional != employeeVM.Address_Additional) ||
                    (employee.Address.City.CityName != employeeVM.City) ||
                    (employee.Address.City.Region.RegionName != employeeVM.Region) ||
                    (employee.Address.City.Region.Country.CountryName != employeeVM.Country))
                {
                    employee.AddressId = addressService.GetAddress(employeeVM.Address,
                        employeeVM.Address_Additional != null ? employeeVM.Address_Additional : null,
                        employeeVM.City,
                        employeeVM.Region,
                        employeeVM.Postal_Code,
                        employeeVM.Country).Id;
                }

                context.SaveChanges();

                return "Success";
            }
            catch
            {
                return "Exception";
            }
        }

        public bool DeleteEmployee(string username)
        {
            try
            {
                var employee = userRepository.GetUserByUsername(username);

                context.LeftEmployees.Add(new LeftEmployee()
                {
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    ImageSrc = employee.ImageSrc,
                    StartDate = employee.Start_Date,
                    PhoneNumber = employee.PhoneNumber,
                    Gender = employee.Gender,
                    Birthday = employee.Birthday,
                    LeaveDate = DateTime.UtcNow,
                    SalaryId = employee.SalaryId,
                    AddressId = employee.AddressId,
                    RoleId = employee.RoleId.Value,
                    DepartmentId = employee.DepartmentId.Value
                });

                context.SaveChanges();

                context.Users.Remove(employee);

                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<LeftEmployeeVM> GetLeftEmployeesVM()
        {
            var leftEmployees = userRepository.GetLeftEmployees();

            return leftEmployees.Select(left_employee => new LeftEmployeeVM() { 
                FirstName = left_employee.FirstName,
                LastName = left_employee.LastName,
                RoleName = left_employee.Role.RoleName,
                Email = left_employee.Email,
                ImageSrc = left_employee.ImageSrc
            }) .ToList();
        }

        public ViewLeftEmployeeVM GetLeftEmployeeVM(string email)
        {
            return userRepository.GetLeftEmployees().Where(n => n.Email.ToLower().Equals(email.ToLower())).Select(left_employee => new ViewLeftEmployeeVM()
            {
                FirstName = left_employee.FirstName,
                MiddleName = left_employee.MiddleName,
                LastName = left_employee.LastName,
                PhoneNumber = left_employee.PhoneNumber,
                Gender = left_employee.Gender,
                Email = left_employee.Email,
                ImageSrc = left_employee.ImageSrc,
                Address = left_employee.Address.StreetAddress,
                Address_Additional = left_employee.Address.StreetAddressAdditional,
                City = left_employee.Address.City.CityName,
                Region = left_employee.Address.City.Region.RegionName,
                Postal_Code = left_employee.Address.City.PostalCode.PostalCodeNumber,
                Birthday = left_employee.Birthday,
                Start_Date = left_employee.StartDate,
                Left_Date = left_employee.LeaveDate,

                Department = left_employee.Department.DepartmentName,
                Salary = left_employee.Salary.grossSalary.ToString(),
                Role = left_employee.Role.RoleName
            }).SingleOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return userRepository.GetUserByUsername(username);
        }
        
        public string UpdateEmployeeAddress(string username, AddressVM addressVM)
        {
            var employee = userRepository.GetUserByUsername(username);

            try
            {
                if(!validationService.ValidateEmployeeAddress(addressVM))
                {
                    return "ValidationFailed";
                }

                if ((employee.Address.StreetAddress != addressVM.Address) ||
                    (employee.Address.StreetAddressAdditional != addressVM.Additional_Address) ||
                    (employee.Address.City.CityName != addressVM.City) ||
                    (employee.Address.City.Region.RegionName != addressVM.Region) ||
                    (employee.Address.City.Region.Country.CountryName != addressVM.Country))
                {
                    employee.AddressId = addressService.GetAddress(addressVM.Address,
                        addressVM.Additional_Address != null ? addressVM.Additional_Address : null,
                        addressVM.City,
                        addressVM.Region,
                        addressVM.Postal_Code,
                        addressVM.Country).Id;

                    context.SaveChanges();
                }
                return "Success";
            }catch
            {
                return "Exception";
            }
        }
    }
}
