using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class CalendarEventVM
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string description { get; set; }
        public string period { get; set; }
    }
}
