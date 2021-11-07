using HCMApp.Data.Services;
using HCMApp.Data.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly RoleService _roleService;
        private readonly HolidaysService holidaysService;

        public AdminController(DepartmentService departmentService,
            RoleService roleService,
            HolidaysService holidaysService)
        {
            _departmentService = departmentService;
            _roleService = roleService;
            this.holidaysService = holidaysService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/create-department")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create_Department()
        {
            //Gets information from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult Submit_Department([Bind] string DepartmentName)
        {
            var dep = DepartmentName;

            RouteValueDictionary rvd = new RouteValueDictionary();

            var result = _departmentService.CreateDepartment(DepartmentName);

            if (result.Equals("Success"))
            {
                rvd.Add("Information", "DepartmentCreated");
            }

            if(result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "ValidationFailed");
            }

            if(result.Equals("DepartmentDuplicate"))
            {
                rvd.Add("Information", "DepartmentDuplicate");
            }

            if(result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Create_Department", rvd);
        }

        [HttpGet("{controller}/create-role")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create_Role()
        {
            //Gets information from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            ViewData["Departments"] = _departmentService.GetDepartments();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Submit_Role([Bind] string Department, string Role, string MinSalary, string MaxSalary,[Bind] bool HR)
        {
            var role = Role;
            var department = Department;

            var isE = HR;

            RouteValueDictionary rvd = new RouteValueDictionary();

            /*var result = _roleService.CreateRole(Role, Convert.ToInt32(Department), MinSalary, MaxSalary);

            if (result.Equals("Success"))
            {
                rvd.Add("Information", "RoleCreated");
            }

            if (result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "ValidationFailed");
            }

            if (result.Equals("DepartmentDuplicate"))
            {
                rvd.Add("Information", "RoleDuplicate");
            }

            if (result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }*/

            return RedirectToAction("Create_Role", rvd);
        }

        [HttpGet("{controller}/departments")]
        [Authorize(Roles = "Admin")]
        public IActionResult Departments()
        {
            ViewData["Departments"] = _departmentService.GetDepartments().OrderBy(n => n.DepartmentName).ToList();
            return View();
        }

        [HttpGet("{controller}/roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult Roles()
        {
            ViewData["Roles"] = _roleService.GetRoles().OrderBy(n => n.Department.DepartmentName).ThenBy(n => n.RoleName).ToList();
            return View();
        }

        [HttpGet("{controller}/holidays")]
        [Authorize(Roles = "Admin")]
        public IActionResult Holidays()
        {
            var holidaysVM = holidaysService.GetHolidayVMsForTheYear(DateTime.UtcNow.Year);
            return View(holidaysVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Submit_Holidays([Bind] List<HolidayVM> holidayVMs)
        {
            var test = holidayVMs;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}
