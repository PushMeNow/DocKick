using System.Security.Claims;
using System.Threading.Tasks;
using DocKick.Data.Entities.Users;
using DocKick.Exceptions;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;

        public AccountService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ExternalUserInfoModel> GetUserInfoFromExternalCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            ExceptionHelper.ThrowIfNull<ExternalAuthenticationException>(info);

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            ExceptionHelper.ThrowIfNull<ExternalAuthenticationException>(result);

            if (!result.Succeeded)
            {
                ExceptionHelper.ThrowExternalAuthentication();
            }

            return new ExternalUserInfoModel
                   {
                       Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                       UserName = info.Principal.FindFirstValue(ClaimTypes.Name)
                   };
        }

        public async Task<IdentityResult> ExternalLogin(ExternalUserInfoModel model)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

            if (user is not null)
            {
                return IdentityResult.Success;
            }

            user = new User
                   {
                       Email = model.Email,
                       UserName = model.Email
                   };

            var result = await _signInManager.UserManager.CreateAsync(user);

            return result;
        }
    }
}