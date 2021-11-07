using HCMApp.Data.Repositories;
using HCMApp.Data.Services;
using HCMApp.Data.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    public class HRController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly UserService _userService;
        private readonly LoginService _loginService;
        private readonly SalaryService _salaryService;
        private readonly AddressService _addressService;
        private readonly ImageService _imageService;
        private readonly CountryService countryService;
        private readonly AbsenceService _absenceService;
        private readonly HolidaysService _holidaysService;
        private readonly WorkMonthService workMonthService;

        public HRController(IUserRepository userRepository, 
            IDepartmentRepository departmentRepository, 
            UserService userService, 
            LoginService loginService,
            SalaryService salaryService,
            AddressService addressService,
            ImageService imageService,
            CountryService countryService,
            AbsenceService absenceService,
            HolidaysService holidaysService,
            WorkMonthService workMonthService)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _userService = userService;
            _loginService = loginService;
            _salaryService = salaryService;
            _addressService = addressService;
            _imageService = imageService;
            this.countryService = countryService;
            _absenceService = absenceService;
            _holidaysService = holidaysService;
            this.workMonthService = workMonthService;
        }

        [Authorize(Roles = "HR")]
        public IActionResult Index()
        {
            //Displays the employee full name on the homepage
            ViewData["UserFullName"] = _userRepository.GetUsersNamesByUsername(HttpContext.User.Identity.Name);

            //Displays the statistics on the HR dashboard
            ViewData["EmployeesAverageAge"] = _userRepository.GetAverageEmployeeAge();
            ViewData["EmployeesAverageSalary"] = _userRepository.GetAverageEmployeeSalary();
            ViewData["EmployeesCount"] = _userRepository.GetEmployeeCount();
            ViewData["LeftEmployeesCount"] = _userRepository.GetLeftEmployeesCount();

            //Displays the remaining paid leave days for the current year
            var pendingLeaveRequests = _absenceService.GetPendingAbsenceRequestVMs();
            ViewData["pendingLeaveRequests"] = pendingLeaveRequests.Count;

            //Gets message from the query if it is available
            ViewData["SalaryApproval"] = HttpContext.Request.Query["SalaryApproval"].ToString();
            return View();
        }

        [HttpGet("profile")]
        [Authorize(Roles = "HR")]
        public IActionResult Profile()
        {
            //Find current logged in user and send it to the view
            var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);

            //Gets message from the query if it is available
            string passwordChanged = HttpContext.Request.Query["passwordChanged"].ToString();
            ViewData["PasswordChanged"] = passwordChanged;
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View(user);
        }

        [HttpGet("add-employee")]
        [Authorize(Roles = "HR")]
        public IActionResult Add_Employee()
        {
            //Get all departments from the repository and send them to the view
            var departments = _departmentRepository.GetDepartments().OrderBy(n => n.DepartmentName).ToList();
            ViewData["Departments"] = departments;

            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View();
        }

        [HttpGet("all-employees")]
        [Authorize(Roles = "HR")]
        public IActionResult All_Employees()
        {
            //Gets all the employees from the database without the current employee
            var allEmployees = _userService.GetUsersWithoutCurrentUser(HttpContext.User.Identity.Name);

            //Sends the user type to the view to render the correct layout
            ViewData["UserType"] = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);

            // Gets message from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            if (!info.Equals(""))
            {
                ViewData["Information"] = info;
            }

            return View(allEmployees);
        }

        [HttpGet("view-employee")]
        [Authorize(Roles = "HR")]
        public IActionResult View_Employee([FromQuery] string Email)
        {
            //Gets the User view model by email and sends it to the view
            var user = _userRepository.GetUserVMByEmail(Email);

            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View(user);
        }

        [HttpGet("employees-by-department")]
        [Authorize(Roles = "HR")]
        public IActionResult Employees_By_Department()
        {
            //Gets all the departments and sends them to the view
            var departments = _departmentRepository.GetDepartments();
            ViewData["Departments"] = departments;

            return View();
        }

        [HttpGet("employees-by-department/view")]
        [Authorize(Roles = "HR")]
        public IActionResult ViewEmployeesByDepartment([FromQuery] string department)
        {
            //Gets all the departments and sends them to the view
            var departments = _departmentRepository.GetDepartments();
            ViewData["Departments"] = departments;

            //Gets the selected department from the query
            ViewData["SelectedDepartment"] = department;

            //Finds the currently logged in user
            var currentUser = HttpContext.User.Identity.Name;

            //Gets all the employees from a department without the current user and sends it to the view
            var allEmployeesInDepartment = _userService.GetEmployeesFromDepartmentWithoutCurrentUser(currentUser, department);

            return View("Employees_By_Department", allEmployeesInDepartment);
        }

        [HttpGet("employees-by-salary")]
        [Authorize(Roles = "HR")]
        public IActionResult Employees_By_Salary()
        {
            return View();
        }

        [HttpGet("employees-by-salary/view")]
        [Authorize(Roles = "HR")]
        public IActionResult ViewEmployeesBySalary([FromQuery] int minimumSalary, int maximumSalary)
        {
            //Finds the currently logged in user
            var currentUser = HttpContext.User.Identity.Name;

            //Gets the employees with salary within minimum and maximum range without the current user
            //and sends it to the view
            var foundEmployees = _userService.GetEmployeesForSalaryRangeWithoutCurrentUser(currentUser, minimumSalary, maximumSalary);

            return View("Employees_By_Salary", foundEmployees);
        }

        [HttpPost("edit-employee")]
        [Authorize(Roles = "HR")]
        public IActionResult Edit_Employee([Bind] ViewEmployeesVM employeeVM)
        {
            //Gets all the departments and sends them to the view
            var departments = _departmentRepository.GetDepartments();
            ViewData["Departments"] = departments;

            //Gets the user view model by the email from the query
            var employee = _userRepository.GetUserVMByEmail(employeeVM.Email);

            //Gets the countries view models and sends them to the view
            var countries = countryService.GetCountriesVM();
            ViewData["Countries"] = countries;

            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Edit_Employee_Submit([Bind] EditEmployeeVM employeeVM)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if the user is successfully updated and redirects to the index page
            var result = _userService.UpdateEmployee(HttpContext, employeeVM);

            if (result.Equals("Success"))
            {
                rvd.Add("Information", "EditSuccess");
            }

            if (result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "ValidationFailed");
                return RedirectToAction("Edit_Employee", rvd);
            }
            
            if(result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("All_Employees", "HR", rvd);
        }

        [HttpPost("change-address")]
        [Authorize(Roles = "HR")]
        public IActionResult Change_Address()
        {
            //Sends the user type to the view to render the correct layout
            ViewData["UserType"] = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);

            //Gets the current user address and sends it to the view
            var currentUserAddress = _addressService.GetCurrentUserAddress(HttpContext.User.Identity.Name);

            //Gets the countries view model and sends it to the view
            var countries = countryService.GetCountriesVM();
            ViewData["Countries"] = countries;

            return View(currentUserAddress);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
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

            if(result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Profile", "HR", rvd);
        }

        [HttpGet]
        [Authorize(Roles = "HR")]
        public IActionResult Change_Image()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Submit_Image_Changes()
        {
            //Gets the current user
            var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);

            //Gets the file from the request
            var files = HttpContext.Request.Form.Files;

            //Checks if there are any files
            if(files.Count > 0)
            {
                //Saves the current user image
                _imageService.SaveImage(HttpContext, HttpContext.User.Identity.Name);
            }


            return View("Profile", user);
        }

        [HttpGet("change-password")]
        [Authorize(Roles = "HR")]
        public IActionResult Change_Password()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Submit_Password_Changes([Bind] PasswordVM passwordVM)
        {
            //Gets the current user
            var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            RouteValueDictionary rvd = new RouteValueDictionary();

            //Checks if the password is sucessfully changed and redirects to the profile page
            var result = _userService.ChangeUserPassword(user, passwordVM);
            if (result.Equals("Success"))
            {
                rvd.Add("Information", "PasswordChanged");
            }

            if(result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "PasswordChanged");
            }
            

            if(result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Profile", "HR", rvd);
        }

        [HttpGet("active-absence-requests")]
        [Authorize (Roles = "HR")]
        public IActionResult Active_Absence_Requests()
        {
            //Gets list of the pending absence requests
            var activeAbsenceRequests = _absenceService.GetPendingAbsenceRequestVMs();

            //Gets information from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View(activeAbsenceRequests);
        }

        [HttpGet("finished-absence-requests")]
        [Authorize(Roles="HR")]
        public IActionResult Finished_Absence_Requests()
        {
            //Gets list of the finished absence requests
            var finishedAbsenceRequests = _absenceService.GetFinishedAbsenceRequestsVMs();

            return View(finishedAbsenceRequests);
        }

        [HttpPost]
        [Authorize (Roles = "HR")]
        public IActionResult Approve_Absence_Request([Bind] AbsenceRequestVM absenceRequestVM)
        {
            var result = _absenceService.ApproveAbsenceRequest(absenceRequestVM);
            RouteValueDictionary rvd = new RouteValueDictionary();

            //Checks if the approval of absence request is successful
            //and redirects to the page with active absence requests
            if (result)
            {
                rvd.Add("Information", "ApproveSuccess");
            } 
            else
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Active_Absence_Requests", rvd);
        }

        [HttpGet("/calendar-absences")]
        [Authorize( Roles = "HR")]
        public IActionResult Calendar_Events()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View();
        }

        [HttpGet("hr/approve-salaries")]
        [Authorize(Roles = "HR")]
        public IActionResult Approve_Salaries()
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if the salary approval for the previous month is available right now
            if (_salaryService.IsSalaryApprovalActive())
            {
                //Gets list of departments and sends it to the view
                var departments = _departmentRepository.GetDepartments();
                ViewData["Departments"] = departments;

                //Gets list of all employees and sends it to the view
                var employees = _userRepository.GetUsers();
                ViewData["Employees"] = employees;

                //Gets list of all employee salary history view model and sends it to the view
                var employeeSalaryHistoryVMs = _salaryService.GetEmployeeSalaryHistoryVMs();

                return View(employeeSalaryHistoryVMs);
            }
            else
            {
                rvd.Add("SalaryApproval", "NotAvailable");
                return RedirectToAction("Index", "HR", rvd);
            }
        }

        [HttpPost]
        [Authorize(Roles ="HR")]
        public IActionResult Submit_Salary_Approval([Bind] List<EmployeeSalaryHistoryVM> employeeSalaryHistories)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            //Checks if the salary approval is successful and redirects to the index page
            if(_salaryService.SubmitSalaryHistoryForMonth(employeeSalaryHistories, DateTime.UtcNow.Month - 1))
            {
                rvd.Add("SalaryApproval", "Success");
            } else
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/view-salary-history")]
        [Authorize(Roles = "HR")]
        public IActionResult Salary_History()
        {
            //Gets the salary history entries for the current user
            var salaryHistoriesForUser = _salaryService.GetEmployeeSalaryHistoriesVM(HttpContext.User.Identity.Name);

            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            return View(salaryHistoriesForUser);
        }

        [HttpGet("request-absence")]
        [Authorize(Roles = "HR")]
        public IActionResult Request_Absence()
        {
            //Sends the user type to the view to render the correct layout
            var currentUserType = _userService.GetCurrentUserType(HttpContext.User.Identity.Name);
            ViewData["UserType"] = currentUserType;

            //Gets information from the query if it is available
            string info = HttpContext.Request.Query["Information"].ToString();
            ViewData["Information"] = info;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
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

            if (absenceRequestResult.Equals("Success"))
            {
                rvd.Add("Information", "Success");
                return RedirectToAction("Request_Absence", rvd);
            }

            return View("Index");
        }

        [HttpGet("absence-requests")]
        [Authorize(Roles = "HR")]
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
        [Authorize(Roles = "HR")]
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

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Delete_Employee([Bind] string username)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            var result = _userService.DeleteEmployee(username);
            //Checks if the deleting of the employee is successful
            if(result)
            {
                rvd.Add("Information", "UserRemoved");
            } else
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("All_Employees", rvd);
        }

        [HttpGet("left-employees)")]
        [Authorize(Roles = "HR")]
        public IActionResult Left_Employees()
        {
            //Gets list of the left employees and sends it to the view
            var leftEmployees = _userService.GetLeftEmployeesVM();

            return View(leftEmployees);
        }

        [HttpGet("view-left-employee")]
        [Authorize(Roles = "HR")]
        public IActionResult View_Left_Employee([FromQuery] string Email)
        {
            //Gets the left employee view model for a specific email
            //and sends it to the view
            var user = _userService.GetLeftEmployeeVM(Email);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult AddEmployee([Bind] AddEmployeeVM employeeVM, string Salary)
        {
            //Add the employee to the database
            RouteValueDictionary rvd = new RouteValueDictionary();

            //Checks if the add user operation is successful and redirects to the all employees page
            var result = _userService.AddUser(HttpContext, employeeVM);

            if(result.Equals("Success"))
            {
                rvd.Add("Information", "UserCreated");
            }
            
            if(result.Equals("ValidationFailed"))
            {
                rvd.Add("Information", "ValidationFailed");
                return RedirectToAction("Add_Employee", rvd);
            }

            if(result.Equals("DuplicateUser"))
            {
                rvd.Add("Information", "UserDuplicate");
            }

            if(result.Equals("Exception"))
            {
                rvd.Add("Information", "Error");
            }

            return RedirectToAction("All_Employees", rvd);
        }
    }
}
