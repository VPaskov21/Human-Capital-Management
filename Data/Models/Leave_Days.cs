using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Leave_Days
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int TotalLeaveDays { get; set; }
        public int AvailableLeaveDays { get; set; }

        //Navigation properties
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
