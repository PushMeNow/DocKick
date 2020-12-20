using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DocKick.Data.Entities.Users;
using DocKick.DataTransferModels.User;
using DocKick.Exceptions;
using DocKick.Services.Settings;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DocKick.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthSettings _authSettings;
        private readonly SignInManager<User> _signInManager;

        public AccountService(SignInManager<User> signInManager, AuthSettings authSettings)
        {
            _signInManager = signInManager;
            _authSettings = authSettings;
        }

        // public async Task<ExternalUserInfoModel> GetUserInfoFromExternalCallback()
        // {
        //     var info = await _signInManager.GetExternalLoginInfoAsync();
        //
        //     ExceptionHelper.ThrowIfNull<ExternalAuthenticationException>(info);
        //
        //     var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //
        //     ExceptionHelper.ThrowIfNull<ExternalAuthenticationException>(result);
        //
        //     if (!result.Succeeded)
        //     {
        //         ExceptionHelper.ThrowExternalAuthentication();
        //     }
        //
        //     return new ExternalUserInfoModel
        //            {
        //                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
        //                UserName = info.Principal.FindFirstValue(ClaimTypes.Name)
        //            };
        // }
        //
        // public async Task<IdentityResult> ExternalLogin(ExternalUserInfoModel model)
        // {
        //     var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
        //
        //     if (user is not null)
        //     {
        //         return IdentityResult.Success;
        //     }
        //
        //     user = new User
        //            {
        //                Email = model.Email,
        //                UserName = model.Email
        //            };
        //
        //     var result = await _signInManager.UserManager.CreateAsync(user);
        //
        //     return result;
        // }

        public async Task<AuthenticatedUserResult> Authenticate(string token)
        {
            ExceptionHelper.ThrowIfNull(token, nameof(token));

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
            var user = await _signInManager.UserManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                user = new User
                       {
                           Email = payload.Email,
                           UserName = payload.Email
                       };

                var result = await _signInManager.UserManager.CreateAsync(user);

                ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);
            }

            var claims = new[]
                         {
                             new Claim(JwtRegisteredClaimNames.Email, user.Email)
                         };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.TokenSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var authenticatedUser = new AuthenticatedUserResult
                                    {
                                        Email = user.Email,
                                        Token = new JwtSecurityToken(string.Empty,
                                                                     string.Empty,
                                                                     claims,
                                                                     expires: DateTime.Now.AddSeconds(55 * 60),
                                                                     signingCredentials: credentials)
                                    };

            return authenticatedUser;
        }
    }
}