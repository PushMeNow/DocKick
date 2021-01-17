using System.Linq;
using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IAuthService authService, SignInManager<User> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }

        [HttpPost("sign-up")]
        public async Task<AuthenticatedUserResult> SignUp([FromBody] SignUpModel model)
        {
            return await _authService.SignUp(model);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            var model = new InternalUserAuthModel
                        {
                            ReturnUrl = returnUrl
                        };

            ViewBag.ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).Select(q => q.Name);

            return View(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] InternalUserAuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Login(model);

            return result
                ? Redirect(model.ReturnUrl)
                : View(model);
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var props = _authService.PrepareAuthProperties(provider, returnUrl, Url.Action(nameof(ExternalLoginCallback)));

            return Challenge(props, provider);
        }

        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var returnUrl = await _authService.ExternalLogin();

            return Redirect(returnUrl);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromQuery] string logoutId)
        {
            var logoutRedirectUrl = await _authService.Logout(logoutId, User.GetSubjectId(), User.GetDisplayName());

            return Redirect(logoutRedirectUrl);
        }
    }
}