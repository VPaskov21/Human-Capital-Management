using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class LeftEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageSrc { get; set; }
        public DateTime StartDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime LeaveDate { get; set; }

        //Navigation properties
        public int SalaryId { get; set; }
        public Salary Salary { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
