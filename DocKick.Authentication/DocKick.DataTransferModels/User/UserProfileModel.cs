using System;

namespace DocKick.DataTransferModels.User
{
    public class UserProfileModel : UserProfileRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}