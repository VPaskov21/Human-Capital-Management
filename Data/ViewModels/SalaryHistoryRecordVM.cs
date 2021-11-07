using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class SalaryHistoryRecordVM
    {
        public string GrossSalary { get; set; }
        public int PaidLeaveUsed { get; set; }
        public double PaidLeaveCost { get; set; }
        public int UnpaidLeaveUsed { get; set; }
        public int SickLeaveUsed { get; set; }
        public double SickLeaveCost { get; set; }
        public int OtherLeaveUsed { get; set; }
        public double OtherLeaveCost { get; set; }
        public double MonthBonus { get; set; }
        public double WorkedOutSalary { get; set; }
        public int WorkedOutDays { get; set; }
        public int MonthNum { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int WorkDaysInMonth { get; set; }
        public double TotalSalaryReceived { get; set; }
    }
}
