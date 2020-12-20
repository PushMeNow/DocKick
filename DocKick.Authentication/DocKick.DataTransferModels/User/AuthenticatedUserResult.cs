using System.IdentityModel.Tokens.Jwt;

namespace DocKick.DataTransferModels.User
{
    public class AuthenticatedUserResult
    {
        public string Email { get; set; }
        public JwtSecurityToken Token { get; set; }
    }
}