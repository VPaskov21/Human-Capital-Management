using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Absence_Request
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        //Navigation properties

        public int UserId { get; set; }
        public User User { get; set; }

        public int Absence_ReasonId { get; set; }
        public Absence_Reason Absence_Reason { get; set; }
    }
}
