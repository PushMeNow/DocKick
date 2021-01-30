using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocKick.DataTransferModels.Users;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using DocKick.Extensions;
using DocKick.Services.Constants;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IIdentityServerInteractionService interaction,
                           IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
        }

        public async Task<bool> Login(SingInModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);

            return await InternalLogin(user, model.Password);
        }

        public async Task<string> Logout(string logoutId, string subjectId, string displayName)
        {
            var context = await _interaction.GetLogoutContextAsync(string.IsNullOrEmpty(logoutId)
                                                                       ? await _interaction.CreateLogoutContextAsync()
                                                                       : logoutId);

            ExceptionHelper.ThrowParameterNullIfNull(context, "Incorrect logout request.");

            await _signInManager.SignOutAsync();

            await _events.RaiseAsync(new UserLogoutSuccessEvent(subjectId, displayName));

            return context.PostLogoutRedirectUri;
        }

        public AuthenticationProperties PrepareAuthProperties(string provider, string returnUrl, string redirectUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["returnUrl"] = returnUrl;

            return properties;
        }

        public async Task<string> ExternalLogin()
        {
            var parseResult = await ParseExternalResponse();

            if (parseResult.IsSignedIn)
            {
                await _events.RaiseAsync(await GetSuccessEvent(parseResult.ExternalLoginInfo));

                return parseResult.ReturnUrl;
            }

            var user = await _userManager.FindByNameAsync(parseResult.Email);

            if (user is null)
            {
                const string defaultPass = "12345";

                var createUserResult = await CreateUser(parseResult.Email, defaultPass);

                ExceptionHelper.ThrowIfTrue<AuthenticationException>(!createUserResult.result.Succeeded);

                user = createUserResult.user;
            }
            else
            {
                var identResult = await _userManager.AddLoginAsync(user, parseResult.ExternalLoginInfo);

                ExceptionHelper.ThrowIfTrue<AuthenticationException>(!identResult.Succeeded);
            }

            await AddDefaultUserClaims(user);
            await _signInManager.SignInAsync(user, false);

            await _events.RaiseAsync(await GetSuccessEvent(parseResult.ExternalLoginInfo));

            return parseResult.ReturnUrl;
        }

        public async Task<bool> SignUp(SignUpModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var checkUser = await _userManager.FindByEmailAsync(model.Email);

            ExceptionHelper.ThrowIfNotNull<AuthenticationException>(checkUser);

            var (identResult, user) = await CreateUser(model.Email, model.Password);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!identResult.Succeeded);

            return await InternalLogin(user, model.Password);
        }

        private async Task<UserLoginSuccessEvent> GetSuccessEvent(ExternalLoginInfo info)
        {
            var email = info.Principal.GetEmail();
            var user = await _userManager.FindByEmailAsync(email);

            ExceptionHelper.ThrowNotFoundIfNull(user, nameof(user));

            return new UserLoginSuccessEvent(user.Email, user.Id.ToString(), user.Email);
        }

        private static UserLoginSuccessEvent GetSuccessEvent(User user)
        {
            return new(user.Email, user.Id.ToString(), user.Email);
        }

        private async Task<(IdentityResult result, User user)> CreateUser(string email, string password)
        {
            var user = new User
                       {
                           Email = email,
                           UserName = email
                       };

            var identResult = await _userManager.CreateAsync(user, password);

            return (identResult, user);
        }

        private async Task AddDefaultUserClaims(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any(q => q.Type == ClaimNames.UserId))
            {
                return;
            }

            var userIdClaim = new Claim(ClaimNames.UserId, user.Id.ToString());
            var result = await _userManager.AddClaimAsync(user, userIdClaim);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);
        }

        private async Task<bool> InternalLogin(User user, string password)
        {
            ExceptionHelper.ThrowIfNull<AuthenticationException>(user);

            await AddDefaultUserClaims(user);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);

            await _events.RaiseAsync(GetSuccessEvent(user));

            return true;
        }

        private async Task<ExternalResponseParseResult> ParseExternalResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoIdentityServer();

            ExceptionHelper.ThrowParameterNullIfNull(info, "External login error.");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            var returnUrl = info.AuthenticationProperties.Items["returnUrl"] ?? "~/";

            if (result.Succeeded)
            {
                return new ExternalResponseParseResult
                       {
                           IsSignedIn = false,
                           ExternalLoginInfo = info,
                           Email = null,
                           ReturnUrl = returnUrl
                       };
            }

            var email = info.Principal.GetEmail();

            ExceptionHelper.ThrowParameterNullIfNull(email, "Incorrect email.");

            return new ExternalResponseParseResult
                   {
                       IsSignedIn = false,
                       ExternalLoginInfo = info,
                       Email = email,
                       ReturnUrl = returnUrl
                   };
        }

        private record ExternalResponseParseResult
        {
            public bool IsSignedIn { get; init; }
            public ExternalLoginInfo ExternalLoginInfo { get; init; }
            public string Email { get; init; }
            public string ReturnUrl { get; init; }
        }
    }
}