using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class EmployeeSalaryHistoryVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string GrossSalary { get; set; }
        public int PaidLeaveUsed { get; set; }
        public double PaidLeaveCost { get; set; }
        public int UnpaidLeaveUsed { get; set; }
        public int SickLeaveUsed { get; set; }
        public double SickLeaveCost { get; set; }
        public int OtherLeaveUsed { get; set; }
        public double OtherLeaveCost { get; set; }
        public double MonthBonus { get; set; }
        public double NetSumToReceive { get; set; }
    }
}
