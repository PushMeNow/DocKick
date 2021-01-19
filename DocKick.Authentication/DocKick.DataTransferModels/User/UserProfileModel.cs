﻿using System;

namespace DocKick.DataTransferModels.User
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Profession { get; set; }
    }
}