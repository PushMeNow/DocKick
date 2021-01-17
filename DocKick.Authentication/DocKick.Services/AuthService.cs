using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Exceptions;
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

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id.ToString(), user.Email));

            return true;
        }

        public async Task<string> Logout(string logoutId,
                                         string subjectId,
                                         string displayName)
        {
            var context = await _interaction.GetLogoutContextAsync(string.IsNullOrEmpty(logoutId)
                                                                       ? await _interaction.CreateLogoutContextAsync()
                                                                       : logoutId);

            ExceptionHelper.ThrowParameterNullIfNull(context, "Incorrect logout request.");

            await _signInManager.SignOutAsync();

            await _events.RaiseAsync(new UserLogoutSuccessEvent(subjectId, displayName));

            return context.PostLogoutRedirectUri;
        }

        public async Task<AuthenticatedUserResult> SignUp(SignUpModel model)
        {
            // ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));
            //
            // var checkUser = await _userManager.FindByEmailAsync(model.Email);
            //
            // ExceptionHelper.ThrowIfNotNull<AuthenticationException>(checkUser);
            //
            // var user = new User
            //            {
            //                Email = model.Email,
            //                UserName = model.Email
            //            };
            //
            // var createResult = await _userManager.CreateAsync(user, model.Password);
            //
            // ExceptionHelper.ThrowIfTrue<AuthenticationException>(!createResult.Succeeded);
            //
            // await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
            //
            // return await GetAuthenticatedUserResultAndAddRefreshToken(user);
            return null;
        }
    }
}