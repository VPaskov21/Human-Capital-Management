using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public double grossSalary { get; set; }
        public int netSalary { get; set; }

        //Navigation properties
        public List<User> Users { get; set; }
    }
}
