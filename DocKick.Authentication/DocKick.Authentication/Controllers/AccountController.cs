using System;
using System.Threading.Tasks;
using DocKick.Authentication.Extensions;
using DocKick.DataTransferModels.User;
using DocKick.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<AuthenticatedUserResult> GoogleLogin([FromBody] UserAuthModel model)
        {
            var result = await _accountService.Authenticate(model.TokenId);

            return result;
        }

        [AllowAnonymous]
        [HttpPost("internal-login")]
        public async Task<AuthenticatedUserResult> InternalLogin([FromBody] InternalUserAuthModel model)
        {
            return await _accountService.Authenticate(model);
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<AuthenticatedUserResult> SignUp([FromBody] SignUpModel model)
        {
            var user = User;
            return await _accountService.SignUp(model);
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<UserProfileModel> Profile()
        {
            return await _accountService.GetProfile(User.GetUserId());
        }
    }
}