using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Profession { get; set; }
    }
}