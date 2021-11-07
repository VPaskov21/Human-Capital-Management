using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Absence_Reason
    {
        public int Id { get; set; }
        public string ReasonName { get; set; }
        public int? AdditionalLeaveDays { get; set; }

        //Navigation properties
        public List<Absence_Request> Absence_Requests { get; set; }
    }
}
