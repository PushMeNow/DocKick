using System.IdentityModel.Tokens.Jwt;

namespace DocKick.DataTransferModels.User
{
    public class AuthenticatedUserResult
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}