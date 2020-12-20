using System.ComponentModel.DataAnnotations;

namespace DocKick.DataTransferModels.User
{
    public class InternalUserAuthModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}