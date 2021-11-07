using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
#nullable enable
        public string? StreetAddressAdditional { get; set; }
#nullable disable

        //Nagivation properties
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
