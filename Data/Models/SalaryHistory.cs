using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class SalaryHistory
    {
        public int Id { get; set; }
        public int WorkedOutDays { get; set; }
        public int? PaidLeaveUsed { get; set; }
        public int? UnpaidLeaveUsed { get; set; }
        public int? SickLeaveUsed { get; set; }
        public int? OtherLeaveUsed { get; set; }
        public double? Bonus { get; set; }
        public double TotalSalary { get; set; }


        //Navigation properties
        public int UserId { get; set; }
        public User User { get; set; }

        public int WorkMonthId { get; set; }
        public WorkMonth WorkMonth { get; set; }
    }
}
