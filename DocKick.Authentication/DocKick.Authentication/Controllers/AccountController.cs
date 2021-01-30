using System.Threading.Tasks;
using DocKick.DataTransferModels.Users;
using DocKick.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Route("[controller]")]
    public class AccountController : AuthenticatedController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("profile")]
        public async Task<UserProfileModel> Profile()
        {
            return await _accountService.GetProfile(UserName);
        }

        [HttpPut("profile")]
        public async Task<UserProfileModel> Profile([FromBody] UserProfileRequest model)
        {
            return await _accountService.UpdateProfile(UserName, model);
        }
    }
}