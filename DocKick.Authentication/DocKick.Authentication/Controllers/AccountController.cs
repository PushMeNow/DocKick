using System.Threading.Tasks;
using DocKick.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var userInfo = await _accountService.GetUserInfoFromExternalCallback();
            var loginResult = await _accountService.ExternalLogin(userInfo);

            return Redirect("/");
        }
    }
}