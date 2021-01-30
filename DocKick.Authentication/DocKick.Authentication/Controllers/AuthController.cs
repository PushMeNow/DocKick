using System.Linq;
using System.Threading.Tasks;
using DocKick.DataTransferModels.Users;
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

        [HttpGet("sign-up")]
        public IActionResult SignUp([FromQuery] string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return BadRequest();
            }

            var model = new SignUpModel()
                        {
                            ReturnUrl = returnUrl
                        };

            return View(model);
        }

        [HttpPost("sign-up")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([FromForm] SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var successful = await _authService.SignUp(model);

            return successful
                ? Redirect(model.ReturnUrl)
                : View(model);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            var model = new SingInModel
                        {
                            ReturnUrl = returnUrl
                        };

            ViewBag.ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).Select(q => q.Name);

            return View(model);
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] SingInModel model)
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