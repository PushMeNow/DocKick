using System.IdentityModel.Tokens.Jwt;

namespace DocKick.DataTransferModels.User
{
    public record AuthenticatedUserResult
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}