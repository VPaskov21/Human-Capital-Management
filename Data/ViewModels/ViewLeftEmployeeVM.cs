﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.ViewModels
{
    public class ViewLeftEmployeeVM
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string ImageSrc { get; set; }
        public string Address { get; set; }
        public string Address_Additional { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Postal_Code { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime Left_Date { get; set; }

        //Navigation properties
        public string Department { get; set; }
        public string Salary { get; set; }
        public string Role { get; set; }
    }
}
