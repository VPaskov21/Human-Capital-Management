using HCMApp.Data.Repositories;
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
    public class EmployeeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        private readonly AddressService _addressService;
        private readonly AbsenceService _absenceService;
        private readonly ImageService _imageService;
        private readonly SalaryService salaryService;
        private readonly CountryService _countryService;

        public EmployeeController(IUserRepository userRepository,
            UserService userService,
            AddressService addressService,
            AbsenceService absenceService,
            ImageService imageService,
            SalaryService salaryService,
            CountryService countryService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _addressService = addressService;
            _absenceService = absenceService;
            _imageService = imageService;
            this.salaryService = salaryService;
            _countryService = countryService;
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            //Displays the employee full name on the homepage
            ViewData["UserFullName"] = _userRepository.GetUsersNamesByUsername(HttpContext.User.Identity.Name);

            //Gets the data about leave days
            ViewData["TotalLeaveDays"] = _userService.GetCurrentUserTotalLeaveDays(HttpContext.User.Identity.Name);
            ViewData["AvailableLeaveDays"] = _userService.GetCurrentUserAvailableLeaveDays(HttpContext.User.Identity.Name);
            return View();
        }

        [HttpGet("employee/dashboard")]
        [Authorize(Roles = "Employee")]
        public IActionResult Profile()
        {
            var username = HttpContext.User.Identity.Name;
            var user = _userRepository.GetUserByUsername(username);

            //Gets the parameter from the request if the password was changed successfully or not
            //and sends it to the view
            string passwordChanged = HttpContext.Request.Query["passwordChanged"].ToString();
            ViewData["PasswordChanged"] = passwordChanged;

            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;
            return View(user);
        }

        [HttpGet("employee/all-employees")]
        [Authorize(Roles = "Employee")]
        public IActionResult All_Employees()
        {
            //Gets list of all employees without the current user
            var allEmployees = _userService.GetUsersWithoutCurrentUser(HttpContext.User.Identity.Name);

            //Sends the user type to the view to render the correct layout
            ViewData["UserType"] = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);

            return View(allEmployees);
        }

        [HttpPost("employee/change-address")]
        [Authorize(Roles = "Employee")]
        public IActionResult Change_Address([Bind] AddressVM addressVM)
        {
            //Sends the user type to the view to render the correct layout
            ViewData["UserType"] = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);

            //Gets the current user address and sends it to the view
            var currentUserAddress = _addressService.GetCurrentUserAddress(HttpContext.User.Identity.Name);

            //Gets the countries view model and sends it to the view
            var countries = _countryService.GetCountriesVM();
            ViewData["Countries"] = countries;

            return View(currentUserAddress);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Submit_Address_Changes([Bind] AddressVM addressVM)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if the user address is successfully updated and redirects to the profile page
            var result = _userService.UpdateEmployeeAddress(HttpContext.User.Identity.Name, addressVM);
            if (result.Equals("Success"))
            {
                rvd.Add("Information", "AddressChanged");
            }

            if (result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "ValidationFailed");
            }

            if (result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Profile", "Employee", rvd);
        }

        [HttpGet("employee/view-employee")]
        [Authorize(Roles = "Employee")]
        public IActionResult View_Employee([FromQuery] string Email)
        {
            //Gets the User view model by email and sends it to the view
            var user = _userRepository.GetUserVMByEmail(Email);

            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View(user);
        }

        [HttpGet("employee/view-employees-for-department")]
        [Authorize(Roles = "Employee")]
        public IActionResult Employees_By_Department()
        {
            //Gets the current user by username
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = _userRepository.GetUserByUsername(currentUserName);

            //Gets list of all the employees in the department without the current user
            //and sends it to the view
            var allEmployeesInDepartment = _userService.GetEmployeesFromDepartmentWithoutCurrentUser(currentUserName, currentUser.Department.DepartmentName);
            ViewData["CurrentEmployeeDepartment"] = currentUser.Department.DepartmentName;

            return View(allEmployeesInDepartment);
        }

        [HttpGet("employee/profile/change-password")]
        [Authorize(Roles = "Employee")]
        public IActionResult Change_Password()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Submit_Password_Changes([Bind] PasswordVM passwordVM)
        {
            var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            RouteValueDictionary rvd = new RouteValueDictionary();

            //Checks if the password is sucessfully changed and redirects to the profile page
            var result = _userService.ChangeUserPassword(user, passwordVM);
            if (result.Equals("Success"))
            {
                rvd.Add("Information", "PasswordChanged");
            }

            if (result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "PasswordChanged");
            }

            if (result.Equals("Exception"))
            {
                rvd.Add("Information", "Exception");
            }

            return RedirectToAction("Profile", "Employee", rvd);
        }

        [HttpGet("employee/request-absence")]
        [Authorize(Roles = "Employee")]
        public IActionResult Request_Absence()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;


            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Submit_Absence_Request([Bind] AbsenceRequestVM absenceRequestVM)
        {
            //Sends the user type to the view to render the correct layout
            ViewData["UserType"] = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);

            var absenceRequestResult = _absenceService.AddAbsence(HttpContext.User.Identity.Name, absenceRequestVM);
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if there are enough paid leave days
            if (absenceRequestResult.Equals("Not enough days available"))
            {
                rvd.Add("Information", "No available days");
                return RedirectToAction("Request_Absence", rvd);
            }

            if(absenceRequestResult.Equals("Success"))
            {
                rvd.Add("Information", "Success");
                return RedirectToAction("Request_Absence", rvd);
            }
            return View("Index");
        }

        [HttpGet("employee/absence-requests")]
        [Authorize(Roles = "Employee")]
        public IActionResult Absence_Requests()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            //Gets list of absence request for the current user
            var absenceRequestsForEmployee = _absenceService.GetAbsenceRequestVMsForEmployee(HttpContext.User.Identity.Name);

            //Gets information from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View(absenceRequestsForEmployee);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Cancel_Absence_Request([Bind] AbsenceRequestVM absenceRequestVM)
        {
            var result = _absenceService.CancelAbsenceRequest(absenceRequestVM);
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if the cancelling of the absence request is successful
            if (result)
            {
                rvd.Add("Information", "CancelSuccess");
            }
            else
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Absence_Requests", rvd);
        }

        [HttpGet("employee/profile/change-image")]
        [Authorize(Roles = "Employee")]
        public IActionResult Change_Image()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Submit_Image_Changes()
        {
            //Gets the current user
            var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);

            //Gets the file from the requestv
            var files = HttpContext.Request.Form.Files;

            //Checks if there are any files
            if (files.Count > 0)
            {
                //Saves the current user image
                _imageService.SaveImage(HttpContext, HttpContext.User.Identity.Name);
            }


            return View("Profile", user);
        }

        [HttpGet("employee/view-salary-history")]
        [Authorize(Roles ="Employee")]
        public IActionResult Salary_History()
        {
            //Gets the salary history entries for the current user
            var salaryHistoriesForUser = salaryService.GetEmployeeSalaryHistoriesVM(HttpContext.User.Identity.Name);

            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View(salaryHistoriesForUser);
        }

        [HttpGet("employee/calendar-absences")]
        [Authorize(Roles = "Employee")]
        public IActionResult Calendar_Events()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}
