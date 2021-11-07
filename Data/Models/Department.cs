using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        //Navigation properties
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
    }
}
