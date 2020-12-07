using System.Security.Claims;
using System.Threading.Tasks;
using DocKick.Exceptions;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ExternalUserInfoModel> ExternalSignIn()
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
    }
}