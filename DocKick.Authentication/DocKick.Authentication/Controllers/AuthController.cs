using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using DocKick.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
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

        [HttpPost("internal-login")]
        public async Task<AuthenticatedUserResult> InternalLogin([FromBody] InternalUserAuthModel model)
        {
            return await _authService.Authenticate(model);
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
    }
}