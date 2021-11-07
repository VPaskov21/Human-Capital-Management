using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public double MinimalSalary { get; set; }
        public double MaximumSalary { get; set; }

        //Navigation properties
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
