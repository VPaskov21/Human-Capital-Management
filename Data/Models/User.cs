using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string ImageSrc { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Start_Date { get; set; }

        //Navigation properties
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int SalaryId { get; set; }
        public Salary Salary { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<Leave_Days> Leave_Days { get; set; }

        public List<Absence_Request> Absence_Requests { get; set; }

        public List<SalaryHistory> SalaryHistories { get; set; }
    }
}
