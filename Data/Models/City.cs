using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        //Navigation properties
        public List<Address> Addresses { get; set; }

        public int PostalCodeId { get; set; }
        public PostalCode PostalCode { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
