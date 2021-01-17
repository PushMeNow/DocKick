using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
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
            return await _accountService.GetProfile(User.Identity.Name);
        }
    }
}