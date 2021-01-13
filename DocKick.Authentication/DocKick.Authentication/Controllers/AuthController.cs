using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Services;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("google-login")]
        public async Task<AuthenticatedUserResult> GoogleLogin([FromBody] UserAuthModel model)
        {
            var result = await _authService.Authenticate(model.TokenId);

            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<AuthenticatedUserResult> RefreshToken(RefreshTokenModel model)
        {
            return await _authService.RefreshToken(model);
        }

        [HttpPost("sign-up")]
        public async Task<AuthenticatedUserResult> SignUp([FromBody] SignUpModel model)
        {
            return await _authService.SignUp(model);
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            var model = new InternalUserAuthModel { ReturnUrl = returnUrl };

            return View(model);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] InternalUserAuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Authenticate(model);

            return result 
                ? Redirect(model.ReturnUrl)
                : View(model);
        }
        
        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromQuery] string logoutId)
        {
            var logoutRedirectUrl = await _authService.Logout(logoutId, User.GetSubjectId(), User.GetDisplayName());

            return Redirect(logoutRedirectUrl);
        }

    }
}