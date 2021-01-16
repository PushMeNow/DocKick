using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DocKick.Data.Repositories;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using DocKick.Services.Constants;
using DocKick.Services.Settings;
using Microsoft.IdentityModel.Tokens;

namespace DocKick.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthSettings _authSettings;

        public TokenService(AuthSettings authSettings)
        {
            _authSettings = authSettings;
        }

        public Task<string> GenerateAccessToken(User user)
        {
            return Task.Run(() =>
                            {
                                var claims = new[]
                                             {
                                                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                                 new Claim(ClaimNames.UserId, user.Id.ToString())
                                             };

                                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.TokenSecret));
                                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                                var token = new JwtSecurityToken(string.Empty,
                                                                 string.Empty,
                                                                 claims,
                                                                 expires: DateTime.Now.AddSeconds(60 * _authSettings.AccessTokenLifeTime),
                                                                 signingCredentials: credentials);

                                var handler = new JwtSecurityTokenHandler();

                                return handler.WriteToken(token);
                            });
        }

        public Task<string> GenerateRefreshToken()
        {
            return Task.Run(() =>
                            {
                                var randomNumber = new byte[32];
                                using var rng = RandomNumberGenerator.Create();

                                rng.GetBytes(randomNumber);

                                return Convert.ToBase64String(randomNumber);
                            });
        }

        public async Task CreateRefreshToken(User user, string refreshToken)
        {
            // var entity = new RefreshToken
            //              {
            //                  Token = refreshToken,
            //                  UserId = user.Id
            //              };
            //
            // entity.UpdateExpiration(_authSettings.RefreshTokenLifeTime);
            //
            // await _repository.Create(entity);
            // await _repository.Save();
        }

        public async Task UpdateRefreshToken(User user, string refreshToken)
        {
            // var entity = await _repository.GetByToken(refreshToken);
            //
            // ExceptionHelper.ThrowNotFoundIfNull(entity, "Refresh token");
            //
            // entity.UpdateExpiration(_authSettings.RefreshTokenLifeTime);
            //
            // _repository.Update(entity);
            //
            // await _repository.Save();
        }

        public async Task DeleteRefreshToken(string refreshToken)
        {
            // await _repository.DeleteByToken(refreshToken);
            // await _repository.Save();
        }

        public async Task ClearUserRefreshTokens(User user)
        {
            // ExceptionHelper.ThrowArgumentNullIfNull(user,nameof(user));
            //
            // await _repository.ClearUserRefreshTokens(user.Id);
            // await _repository.Save();
        }

        public async Task<bool> ValidateRefreshToken(User user, string refreshToken)
        {
            // ExceptionHelper.ThrowArgumentNullIfNull(user, nameof(user));
            // ExceptionHelper.ThrowArgumentNullIfNull(refreshToken, nameof(refreshToken));
            //
            // var entity = await _repository.GetByToken(refreshToken);
            //
            // ExceptionHelper.ThrowNotFoundIfNull(entity, "Refresh token");
            //
            return false;
        }

        public Task<Guid> GetUserIdFromAccessToken(string accessToken)
        {
            return Task.Run(() =>
                            {
                                var tokenHandler = new JwtSecurityTokenHandler();

                                var tokenValidationParams = new TokenValidationParameters
                                                            {
                                                                ValidateAudience = false,
                                                                ValidateIssuer = false,
                                                                ValidateActor = false,
                                                                ValidateLifetime = false,
                                                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.TokenSecret))
                                                            };

                                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out var securityToken);

                                var isValidToken = !(securityToken is JwtSecurityToken jwtSecurityToken)
                                                   || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                                                          StringComparison.InvariantCultureIgnoreCase);

                                ExceptionHelper.ThrowTokenExceptionIfTrue(isValidToken, "Access token is invalid");

                                var userId = principal.FindFirstValue(ClaimNames.UserId);

                                if (string.IsNullOrEmpty(userId))
                                {
                                    throw new SecurityTokenException($"Missing claim: {ClaimTypes.Name}!");
                                }

                                return Guid.Parse(userId);
                            });
        }
    }
}