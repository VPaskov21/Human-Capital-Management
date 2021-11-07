using HCMApp.Data.Models;
using HCMApp.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                context.Database.Migrate();
                var salaryService = scope.ServiceProvider.GetService<SalaryService>();
                var addressService = scope.ServiceProvider.GetService<AddressService>();
                var userService = scope.ServiceProvider.GetService<UserService>();
                var holidaysService = scope.ServiceProvider.GetService<HolidaysService>();
                var departmentService = scope.ServiceProvider.GetService<DepartmentService>();
                var workMonthService = scope.ServiceProvider.GetService<WorkMonthService>();

                SeedSalaries(context, salaryService);

                SeedDepartments(context);

                SeedRoles(context);

                SeedUserTypes(context);

                SeedCountries(context);

                SeedRegions(context);

                SeedPostalCodes(context);

                SeedCities(context);

                SeedAddresses(context);

                SeedAbsenceReasons(context);

                SeedUsers(context, salaryService, addressService, departmentService);

                SeedEmployeeLeaveDays(context, userService);

                SeedHolidays(context, holidaysService);

                SeedWorkMonths(context, workMonthService);

                SeedLeftEmployees(context, salaryService, addressService, departmentService);

                SeedEmployeeAbsenceRequests(context, userService);

                SeedSalaryHistory(context, salaryService, userService);

                SeedAdminUser(context);
            }
        }

        private static void SeedAdminUser(AppDbContext context)
        {
            if(!context.AdminUsers.Any())
            {
                context.AdminUsers.Add(new AdminUser()
                {
                    Username = "admin",
                    Password = "pass",
                });

                context.SaveChanges();
            }
        }

        private static void SeedSalaries(AppDbContext context, SalaryService salaryService)
        {
            if (!context.Salaries.Any())
            {
                context.Salaries.Add(new Salary()
                {
                    grossSalary = 1288.69,
                    netSalary = (int)salaryService.CalculateNetSalary(1288.69)
                });

                context.Salaries.Add(new Salary()
                {
                    grossSalary = 1546.43,
                    netSalary = (int)salaryService.CalculateNetSalary(1546.43)
                });

                context.Salaries.Add(new Salary()
                {
                    grossSalary = 1933.05,
                    netSalary = (int)salaryService.CalculateNetSalary(1933.05)
                });

                context.Salaries.Add(new Salary()
                {
                    grossSalary = 2319.65,
                    netSalary = (int)salaryService.CalculateNetSalary(2319.65)
                });

                context.Salaries.Add(new Salary()
                {
                    grossSalary = 2577.38,
                    netSalary = (int)salaryService.CalculateNetSalary(2577.38)
                });

                context.SaveChanges();
            }
        }

        private static void SeedDepartments(AppDbContext context)
        {
            if (!context.Departments.Any())
            {
                context.Departments.Add(new Department()
                {
                    DepartmentName = "Човешки Ресурси"
                });

                context.Departments.Add(new Department()
                {
                    DepartmentName = "Разработки"
                });

                context.Departments.Add(new Department()
                {
                    DepartmentName = "Маркетинг"
                });

                context.Departments.Add(new Department()
                {
                    DepartmentName = "Продажби"
                });

                context.Departments.Add(new Department()
                {
                    DepartmentName = "Поддръжка"
                });

                context.Departments.Add(new Department()
                {
                    DepartmentName = "Финанси"
                });

                context.SaveChanges();
            }
        }

        private static void SeedRoles(AppDbContext context)
        {
            if(!context.Roles.Any())
            {
                context.Roles.Add(new Role()
                {
                    RoleName = "Специалист човешки ресурси",
                    MinimalSalary = 1546.43,
                    MaximumSalary = 3191.18,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Човешки Ресурси")).Single().Id                    
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Младши системен администратор",
                    MinimalSalary = 1804.17,
                    MaximumSalary = 2448.52,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Поддръжка")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Администратор",
                    MinimalSalary = 1933.05,
                    MaximumSalary = 3635.62,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Поддръжка")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Експерт, маркетинг",
                    MinimalSalary = 1675.30,
                    MaximumSalary = 3746.73,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Маркетинг")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Мрежови администратор",
                    MinimalSalary = 1933.05,
                    MaximumSalary = 4302.29,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Поддръжка")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Мениджър продажби",
                    MinimalSalary = 2577.38,
                    MaximumSalary = 5413.40,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Продажби")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Мениджър реклама",
                    MinimalSalary = 2190.79,
                    MaximumSalary = 4857.85,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Продажби")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Системен администратор",
                    MinimalSalary = 2577.38,
                    MaximumSalary = 7080.07,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Поддръжка")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Програмист, софтуерни приложения",
                    MinimalSalary = 3191.18,
                    MaximumSalary = 9302.29,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Разработки")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Програмист, бази данни",
                    MinimalSalary = 2577.38,
                    MaximumSalary = 7635.62,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Разработки")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Графичен дизайнер",
                    MinimalSalary = 1933.05,
                    MaximumSalary = 6524.51,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Разработки")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Финансов директор",
                    MinimalSalary = 2577.38,
                    MaximumSalary = 7080.07,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Финанси")).Single().Id
                });

                context.Roles.Add(new Role()
                {
                    RoleName = "Финансов мениджър",
                    MinimalSalary = 2577.38,
                    MaximumSalary = 5413.40,
                    DepartmentId = context.Departments.Where(n => n.DepartmentName.Equals("Финанси")).Single().Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedUserTypes(AppDbContext context)
        {
            if(!context.UserTypes.Any())
            {
                context.UserTypes.Add(new UserType()
                {
                    UserTypeName = "HR"
                });

                context.UserTypes.Add(new UserType()
                {
                    UserTypeName = "Employee"
                });

                context.SaveChanges();
            }
        }

        private static void SeedCountries(AppDbContext context)
        {
            if (!context.Countries.Any())
            {
                context.Countries.Add(new Country() 
                { 
                    CountryName = "България"
                });

                context.SaveChanges();
            }
        }

        private static void SeedRegions(AppDbContext context)
        {
            if(!context.Regions.Any())
            {
                context.Regions.Add(new Region()
                {
                    RegionName = "Варна",
                    CountryId = context.Countries.Where(n => n.CountryName.ToLower().Equals("България")).Single().Id
                });

                context.Regions.Add(new Region()
                {
                    RegionName = "София",
                    CountryId = context.Countries.Where(n => n.CountryName.ToLower().Equals("България")).Single().Id
                });

                context.Regions.Add(new Region()
                {
                    RegionName = "Бургас",
                    CountryId = context.Countries.Where(n => n.CountryName.ToLower().Equals("България")).Single().Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedPostalCodes(AppDbContext context)
        {
            if(!context.PostalCodes.Any())
            {
                context.PostalCodes.Add(new PostalCode()
                {
                    PostalCodeNumber = "9000"
                });

                context.PostalCodes.Add(new PostalCode()
                {
                    PostalCodeNumber = "1000"
                });

                context.PostalCodes.Add(new PostalCode()
                {
                    PostalCodeNumber = "8000"
                });

                context.SaveChanges();
            }
        }

        private static void SeedCities(AppDbContext context)
        {
            if(!context.Cities.Any())
            {
                context.Cities.Add(new City()
                {
                    CityName = "Варна",
                    PostalCodeId = context.PostalCodes.Where(n => n.PostalCodeNumber.Equals("9000")).Single().Id,
                    RegionId = context.Regions.Where(n => n.RegionName.ToLower().Equals("Варна".ToLower())).Single().Id
                });

                context.Cities.Add(new City()
                {
                    CityName = "Бургас",
                    PostalCodeId = context.PostalCodes.Where(n => n.PostalCodeNumber.Equals("8000")).Single().Id,
                    RegionId = context.Regions.Where(n => n.RegionName.ToLower().Equals("Бургас".ToLower())).Single().Id
                });

                context.Cities.Add(new City()
                {
                    CityName = "София",
                    PostalCodeId = context.PostalCodes.Where(n => n.PostalCodeNumber.Equals("1000")).Single().Id,
                    RegionId = context.Regions.Where(n => n.RegionName.ToLower().Equals("София".ToLower())).Single().Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedAddresses(AppDbContext context)
        {
            if(!context.Addresses.Any())
            {
                context.Addresses.Add(new Address()
                {
                    StreetAddress = "ул. Христо Ботев 55",
                    CityId = context.Cities.Where(n => n.CityName.ToLower().Equals("Варна".ToLower())).Single().Id
                });

                context.Addresses.Add(new Address()
                {
                    StreetAddress = "ул. Странджа",
                    StreetAddressAdditional = "бл. 2, вх. Б",
                    CityId = context.Cities.Where(n => n.CityName.ToLower().Equals("Варна".ToLower())).Single().Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedAbsenceReasons(AppDbContext context)
        {
            if(!context.Absence_Reasons.Any())
            {
                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Платена отпуска"
                });

                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Неплатена отпуска"
                });

                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Болничен"
                });

                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Брак",
                    AdditionalLeaveDays = 2
                });

                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Кръводаряване",
                    AdditionalLeaveDays = 2
                });

                context.Absence_Reasons.Add(new Absence_Reason()
                {
                    ReasonName = "Погребение",
                    AdditionalLeaveDays = 2
                });

                context.SaveChanges();
            }
        }

        private static void SeedUsers(AppDbContext context, 
            SalaryService salaryService, 
            AddressService addressService,
            DepartmentService departmentService)
        {
            DateTime birthday_Date;
            DateTime start_date;
            if (!context.Users.Any())
            {
                birthday_Date = DateTime.ParseExact("12-03-1989", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("10-02-2019", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Кирил",
                    MiddleName = "Димитров",
                    LastName = "Попов",
                    Username = "KPopov",
                    Password = "123pass",
                    PhoneNumber = "123456789",
                    ImageSrc = "KPopov.jpg",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "k.popov@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Човешки Ресурси".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(1933.05).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Специалист човешки ресурси")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("HR".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Христо Ботев 55", null, "Варна", "Варна", "9000", "България").Id
                });

                birthday_Date = DateTime.ParseExact("16-10-1996", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("02-08-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Пламен",
                    MiddleName = "Иванов",
                    LastName = "Петров",
                    Username = "PIPetrov",
                    Password = "random",
                    PhoneNumber = "0885434777",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "p.petrov@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Поддръжка".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(1546.43).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Младши системен администратор")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Странджа", "бл. 2, вх. Б", "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "PIPetrov.jpg"
                });

                birthday_Date = DateTime.ParseExact("05-07-1991", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("03-02-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Иван",
                    MiddleName = "Петров",
                    LastName = "Стефанов",
                    Username = "IPStefanov",
                    Password = "adminpass",
                    PhoneNumber = "0885343555",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "i.stefanov@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Поддръжка".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(2190.79).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Администратор")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Иван Вазов 14", "вх. Е, ап. 30", "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "IPStefanov.jpg"
                });

                birthday_Date = DateTime.ParseExact("10-09-1996", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-06-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Галин",
                    MiddleName = "Николов",
                    LastName = "Пенчев",
                    Username = "GNPenchev",
                    Password = "random",
                    PhoneNumber = "0885554871",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "g.penchev@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Маркетинг".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(1546.43).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Експерт, маркетинг")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Иван Вазов 14", "вх. Е, ап. 30", "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "GNPenchev.jpg"
                });

                birthday_Date = DateTime.ParseExact("22-03-1986", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Цвета",
                    MiddleName = "Димитрова",
                    LastName = "Тончева",
                    Username = "CDToncheva",
                    Password = "random",
                    PhoneNumber = "0875559728",
                    Gender = "Жена",
                    Birthday = birthday_Date,
                    Email = "c.toncheva@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Финанси".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(5413.40).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Финансов директор")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Рила 34", null, "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "CDToncheva.jpg"
                });

                birthday_Date = DateTime.ParseExact("10-11-1988", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Александра",
                    MiddleName = "Тодорова",
                    LastName = "Антонова",
                    Username = "ATAntonova",
                    Password = "random",
                    PhoneNumber = "0885551748",
                    Gender = "Жена",
                    Birthday = birthday_Date,
                    Email = "a.antonova@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Финанси".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(4524.51).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Финансов мениджър")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Тракия 12", null, "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "ATAntonova.jpg"
                });

                birthday_Date = DateTime.ParseExact("05-12-1991", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Теодор",
                    MiddleName = "Филипов",
                    LastName = "Спасов",
                    Username = "TFSpasov",
                    Password = "random",
                    PhoneNumber = "0895287703",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "t.spasov@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Разработки".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(4024.51).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Програмист, софтуерни приложения")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Хаджи Димитър", null, "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "TFSpasov.jpg"
                });

                birthday_Date = DateTime.ParseExact("17-01-1998", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.Users.Add(new User()
                {
                    FirstName = "Анелия",
                    MiddleName = "Боянова",
                    LastName = "Димитрова",
                    Username = "ABDimitrova",
                    Password = "random",
                    PhoneNumber = "0884477267",
                    Gender = "Жена",
                    Birthday = birthday_Date,
                    Email = "a.dimitrova@example.com",
                    Start_Date = start_date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Разработки".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(2835.12).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Графичен дизайнер")).Single().Id,
                    UserTypeId = context.UserTypes.Where(n => n.UserTypeName.ToLower().Equals("Employee".ToLower())).Single().Id,
                    AddressId = addressService.GetAddress("ул. Доспат 44", null, "Варна", "Варна", "9000", "България").Id,
                    ImageSrc = "ABDimitrova.jpg"
                });

                context.SaveChanges();
            }
        }

        private static void SeedEmployeeLeaveDays(AppDbContext context, UserService userService)
        {
            if (!context.Leave_Days.Any())
            {
                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("KPopov")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("PIPetrov")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("IPStefanov")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("GNPenchev")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("CDToncheva")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("ATAntonova")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("TFSpasov")
                });

                context.Leave_Days.Add(new Leave_Days()
                {
                    Year = DateTime.UtcNow.Year,
                    TotalLeaveDays = 20,
                    AvailableLeaveDays = 20,
                    UserId = userService.GetUserId("ABDimitrova")
                });

                context.SaveChanges();
            }
        }

        private static void SeedEmployeeAbsenceRequests(AppDbContext context, UserService userService)
        {
            if(!context.Absence_Requests.Any())
            {
                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("05-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("08-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n=>n.Username.Equals("PIPetrov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n=>n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("PIPetrov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 3;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("12-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("16-04-2021","dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("PIPetrov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("PIPetrov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("22-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("23-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("PIPetrov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("PIPetrov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 2;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("12-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("16-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("IPStefanov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Болничен")).Single().Id
                });

                //----- G.Penchev absence requests

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("19-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("21-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Неплатена отпуска")).Single().Id
                });

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("08-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("17-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("GNPenchev").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 8;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("22-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("26-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("GNPenchev").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;


                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("21-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("22-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("GNPenchev").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 2;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("04-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("05-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Неплатена отпуска")).Single().Id
                });

                //----- C.Toncheva absence requests

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("24-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("26-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("CDToncheva").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 3;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("05-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("08-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Болничен")).Single().Id
                });

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("22-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("23-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Кръводаряване")).Single().Id
                });

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("25-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("28-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("CDToncheva").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 4;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("16-06-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("18-06-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("CDToncheva").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 3;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("21-06-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("23-06-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Болничен")).Single().Id
                });


                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("09-08-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("13-08-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("CDToncheva")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("CDToncheva").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;

                //------ A.Antonova absence requests

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("25-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("29-01-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ATAntonova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ATAntonova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("08-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("09-02-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ATAntonova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ATAntonova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 2;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("21-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("23-04-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ATAntonova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ATAntonova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 3;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("07-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("07-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ATAntonova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ATAntonova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 1;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("19-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("21-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ATAntonova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ATAntonova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 3;

                //------ T.Spasov absence requests

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("04-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("05-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("TFSpasov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("TFSpasov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 2;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("07-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("07-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("TFSpasov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("TFSpasov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 1;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("26-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("30-07-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("TFSpasov")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("TFSpasov").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;

                //------ A.Dimitrova absence requests

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("08-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("12-03-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ABDimitrova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Платена отпуска")).Single().Id
                });

                userService.GetUserByUsername("ABDimitrova").Leave_Days.Where(n => n.Year == DateTime.UtcNow.Year).SingleOrDefault().AvailableLeaveDays -= 5;

                context.Absence_Requests.Add(new Absence_Request()
                {
                    StartDate = DateTime.ParseExact("19-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact("21-05-2021", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    Status = "approved",
                    UserId = context.Users.Where(n => n.Username.Equals("ABDimitrova")).Single().UserId,
                    Absence_ReasonId = context.Absence_Reasons.Where(n => n.ReasonName.Equals("Болничен")).Single().Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedHolidays(AppDbContext context, HolidaysService holidaysService)
        {
            if(!context.Holidays.Any())
            {
                List<Holiday> holidays = holidaysService.GetHolidaysForTheYear(DateTime.UtcNow.Year);

                context.Holidays.AddRange(holidays);

                context.SaveChanges();
            }
        }

        private static void SeedWorkMonths(AppDbContext context, WorkMonthService workMonthService)
        {
            if(!context.WorkMonths.Any())
            {
                List<WorkMonth> workMonths = workMonthService.GetWorkMonthsForTheYear(DateTime.UtcNow.Year);

                context.WorkMonths.AddRange(workMonths);

                context.SaveChanges();
            }
        }

        private static void SeedLeftEmployees(AppDbContext context,
            SalaryService salaryService,
            AddressService addressService,
            DepartmentService departmentService)
        {
            DateTime birthday_Date;
            DateTime start_date;
            DateTime leave_Date;

            if (!context.LeftEmployees.Any())
            {
                birthday_Date = DateTime.ParseExact("15-10-1992", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                start_date = DateTime.ParseExact("01-03-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                leave_Date = DateTime.ParseExact("01-12-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                context.LeftEmployees.Add(new LeftEmployee()
                {
                    FirstName = "Радомир",
                    MiddleName = "Миленов",
                    LastName = "Ганев",
                    PhoneNumber = "0988555147",
                    ImageSrc = "RMGanev.jpg",
                    Gender = "Мъж",
                    Birthday = birthday_Date,
                    Email = "r.ganev@example.com",
                    StartDate = start_date,
                    LeaveDate = leave_Date,
                    DepartmentId = departmentService.GetDepartments().Where(n => n.DepartmentName.ToLower().Equals("Продажби".ToLower())).Single().Id,
                    SalaryId = salaryService.GetSalaryBySalaryGrossAmount(3191.18).Id,
                    RoleId = context.Roles.Where(n => n.RoleName.Equals("Мениджър продажби")).Single().Id,
                    AddressId = addressService.GetAddress("ул. Петър Райчев 55", "вх. Б, ет. 4", "Варна", "Варна", "9000", "България").Id
                });

                context.SaveChanges();
            }
        }

        private static void SeedSalaryHistory(AppDbContext context, SalaryService salaryService, UserService userService)
        {
            if(!context.SalaryHistories.Any())
            {
                //Add Salary History For GNPenchev employee for January - July period
                var user = userService.GetUserByUsername("GNPenchev");
                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 18,
                    PaidLeaveUsed = 2,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 13,
                    PaidLeaveUsed = 5,
                    UnpaidLeaveUsed = 2,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary -
                    salaryService.CalculateUnpaidLeaveCost(context.WorkMonths.Where(n => n.MonthNumber == 2).Single().WorkDays, 2, user.Salary.grossSalary)),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 14,
                    PaidLeaveUsed = 8,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                }); 

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 21,
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17,
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22,
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSalary(user.Salary.grossSalary),
                    UserId = context.Users.Where(n => n.Username.Equals("GNPenchev")).Single().UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                //Add Salary History For CDToncheva employee for January - July period
                user = userService.GetUserByUsername("CDToncheva");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20,
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 250,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 250),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20,
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 19,
                    PaidLeaveUsed = 3,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 3, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 15, // 21 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 4,
                    OtherLeaveUsed = 2,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 0, 0, 4, 2, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 13, // 17 work days in month
                    PaidLeaveUsed = 4,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 150,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 4, 0, 0, 0, user.Salary.grossSalary, 150),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 19, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 3,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 3, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For ATAntonova employee for January - July period
                user = userService.GetUserByUsername("ATAntonova");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 15, // 20 work days in month
                    PaidLeaveUsed = 5,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 5, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 18, // 20 work days in month
                    PaidLeaveUsed = 2,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 2, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 100,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 0, 0, 0, 0, user.Salary.grossSalary, 100),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 18, // 21 work days in month
                    PaidLeaveUsed = 3,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 3, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 13, // 17 work days in month
                    PaidLeaveUsed = 4,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 4, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 200,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 200),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For TFSpasov employee for January - July period
                user = userService.GetUserByUsername("TFSpasov");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 150,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 150),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 22 work days in month
                    PaidLeaveUsed = 2,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 2, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 21, // 21 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 16, // 17 work days in month
                    PaidLeaveUsed = 1,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 1, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 22 work days in month
                    PaidLeaveUsed = 5,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 5, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For ABDimitrova employee for January - July period
                user = userService.GetUserByUsername("ABDimitrova");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 100,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 100),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 22 work days in month
                    PaidLeaveUsed = 5,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 5, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 21, // 21 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 14, // 17 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 3,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 0, 0, 3, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For KPopov employee for January - July period
                user = userService.GetUserByUsername("KPopov");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 21, // 21 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 200,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 0, 0, 0, 0, user.Salary.grossSalary, 200),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 17 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For PIPetrov employee for January - July period
                user = userService.GetUserByUsername("PIPetrov");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 22 work days in month
                    PaidLeaveUsed = 2,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 2, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 16, // 21 work days in month
                    PaidLeaveUsed = 5,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 5, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 17 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 19, // 22 work days in month
                    PaidLeaveUsed = 3,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 3, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                //Add Salary History For IPStefanov employee for January - July period
                user = userService.GetUserByUsername("IPStefanov");

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(1, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 1 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 20, // 20 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(2, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 2 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(3, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 3 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 21, // 21 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(4, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 4 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 17 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(5, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 5 && n.Year == 2021).Single().Id,
                });

                context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 22, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 0,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(6, 0, 0, 0, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 6 && n.Year == 2021).Single().Id,
                });

                /*context.SalaryHistories.Add(new SalaryHistory()
                {
                    WorkedOutDays = 17, // 22 work days in month
                    PaidLeaveUsed = 0,
                    UnpaidLeaveUsed = 0,
                    SickLeaveUsed = 5,
                    OtherLeaveUsed = 0,
                    Bonus = 0,
                    TotalSalary = salaryService.CalculateNetSumToReceive(7, 0, 0, 5, 0, user.Salary.grossSalary, 0),
                    UserId = user.UserId,
                    WorkMonthId = context.WorkMonths.Where(n => n.MonthNumber == 7 && n.Year == 2021).Single().Id,
                });*/

                context.SaveChanges();
            }
        }
    }
}
