using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class AbsenceRequestVM
    {
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }

        public string Status { get; set; }
    }
}
