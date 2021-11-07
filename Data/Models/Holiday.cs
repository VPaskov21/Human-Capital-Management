using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }

        //if holiday is on weekend
        public DateTime? ActualAbsenceDay { get; set; }
    }
}
