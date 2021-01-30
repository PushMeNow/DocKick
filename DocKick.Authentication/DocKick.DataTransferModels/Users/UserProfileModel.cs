using System;

namespace DocKick.DataTransferModels.Users
{
    public record UserProfileModel : UserProfileRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}