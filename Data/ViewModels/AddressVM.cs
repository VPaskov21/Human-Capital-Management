﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class AddressVM
    {
        public string Address { get; set; }
        public string Additional_Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Postal_Code { get; set; }
        public string Country { get; set; }
    }
}
