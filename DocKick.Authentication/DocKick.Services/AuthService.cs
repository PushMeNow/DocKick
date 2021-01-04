using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedUserResult> Authenticate(string token)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(token, nameof(token));

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                user = new User
                       {
                           Email = payload.Email,
                           UserName = payload.Email
                       };

                var result = await _userManager.CreateAsync(user);

                ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);
            }

            return await GetAuthenticatedUserResultAndAddRefreshToken(user, true);
        }

        public async Task<AuthenticatedUserResult> Authenticate(InternalUserAuthModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);

            ExceptionHelper.ThrowIfNull<AuthenticationException>(user);

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result);

            return await GetAuthenticatedUserResultAndAddRefreshToken(user, true);
        }

        public async Task<AuthenticatedUserResult> SignUp(SignUpModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var checkUser = await _userManager.FindByEmailAsync(model.Email);

            ExceptionHelper.ThrowIfNotNull<AuthenticationException>(checkUser);

            var user = new User
                       {
                           Email = model.Email,
                           UserName = model.Email
                       };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!createResult.Succeeded);

            return await GetAuthenticatedUserResultAndAddRefreshToken(user);
        }

        public async Task<AuthenticatedUserResult> RefreshToken(RefreshTokenModel model)
        {
            var userId = await _tokenService.GetUserIdFromAccessToken(model.AccessToken);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            ExceptionHelper.ThrowNotFoundIfNull(user, "User");
            ExceptionHelper.ThrowTokenExceptionIfTrue(!await _tokenService.ValidateRefreshToken(user, model.RefreshToken), "Refresh token is invalid.");

            await _tokenService.DeleteRefreshToken(model.RefreshToken);

            return await GetAuthenticatedUserResultAndAddRefreshToken(user);
        }

        private async Task<AuthenticatedUserResult> GetAuthenticatedUserResult(User user)
        {
            var authenticatedUser = new AuthenticatedUserResult
                                    {
                                        Email = user.Email,
                                        AccessToken = await _tokenService.GenerateAccessToken(user),
                                        RefreshToken = await _tokenService.GenerateRefreshToken()
                                    };

            return authenticatedUser;
        }

        private async Task<AuthenticatedUserResult> GetAuthenticatedUserResultAndAddRefreshToken(User user, bool clearRefreshTokens = false)
        {
            var result = await GetAuthenticatedUserResult(user);

            if (clearRefreshTokens)
            {
                await _tokenService.ClearUserRefreshTokens(user);
            }

            await _tokenService.CreateRefreshToken(user, result.RefreshToken);

            return result;
        }
    }
}