using System.Threading.Tasks;
using DocKick.Authentication.Extensions;
using DocKick.DataTransferModels.User;
using DocKick.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("profile")]
        public async Task<UserProfileModel> Profile()
        {
            return await _accountService.GetProfile(User.GetUserId());
        }
    }
}