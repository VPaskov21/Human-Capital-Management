using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Models
{
    public class UserType
    {
        public int Id { get; set; }
        public string UserTypeName { get; set; }

        //Navigation properties
        public List<User> Users { get; set; }
    }
}
