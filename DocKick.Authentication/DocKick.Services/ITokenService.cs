using System;
using System.Threading.Tasks;
using DocKick.Entities.Users;

namespace DocKick.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);

        Task<string> GenerateRefreshToken();

        Task CreateRefreshToken(User user, string refreshToken);

        Task<bool> ValidateRefreshToken(User user, string refreshToken);

        Task<Guid> GetUserIdFromAccessToken(string accessToken);

        Task UpdateRefreshToken(User user, string refreshToken);

        Task DeleteRefreshToken(string refreshToken);

        Task ClearUserRefreshTokens(User user);
    }
}