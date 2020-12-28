using System;

namespace DocKick.DataTransferModels.User
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}