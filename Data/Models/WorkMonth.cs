using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class WorkMonth
    {
        public int Id { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public int WorkDays { get; set; }

        //Navigation properties

        public List<SalaryHistory> SalaryHistories { get; set; }
    }
}
