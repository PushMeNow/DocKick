using System.Security.Claims;
using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using Google.Apis.Auth;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEventService _events;

        private readonly IIdentityServerInteractionService _interaction;

        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           ITokenService tokenService,
                           IIdentityServerInteractionService interaction,
                           IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _interaction = interaction;
            _events = events;
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

        public async Task<bool> Authenticate(InternalUserAuthModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);

            ExceptionHelper.ThrowIfNull<AuthenticationException>(user);

            var result = await _signInManager.PasswordSignInAsync(user,
                                                                  model.Password,
                                                                  false,
                                                                  false);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);

            // return await GetAuthenticatedUserResultAndAddRefreshToken(user, true);
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id.ToString(), user.Email));

            return true;
        }

        public async Task<string> Logout(string logoutId,
                                         string subjectId,
                                         string displayName)
        {
            var context = await _interaction.GetLogoutContextAsync(await _interaction.CreateLogoutContextAsync());

            ExceptionHelper.ThrowParameterNullIfNull(context, "Incorrect logout request.");

            await _signInManager.SignOutAsync();

            await _events.RaiseAsync(new UserLogoutSuccessEvent(subjectId, displayName));

            return context.PostLogoutRedirectUri;
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

            await _userManager.AddClaimAsync(user, new Claim("email", user.Email));

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