using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }

        //Navigation properties
        public List<Region> Regions { get; set; }
    }
}
