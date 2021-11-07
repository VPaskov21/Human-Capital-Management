using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class HolidayVM
    {
        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }
        public DateTime? ActualAbsenceDay { get; set; }
    }
}
