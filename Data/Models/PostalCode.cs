using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class PostalCode
    {
        public int Id { get; set; }
        public string PostalCodeNumber { get; set; }

        public City City { get; set; }
    }
}
