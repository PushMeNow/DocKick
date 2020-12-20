using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DocKick.Services;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Route("[controller]")]
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<AuthenticationProviderModel>> GetProviders()
        {
            return await _providerService.GetProviders();
        }

        [HttpGet("external-login")]
        [ProducesResponseType(typeof(ChallengeResult), (int)HttpStatusCode.OK)]
        public IActionResult ExternalLogin([FromQuery] string providerName)
        {
            var redirectUrl = Url.Action(nameof(AccountController.ExternalLoginCallback), "Account");
            var properties = _providerService.GetAuthenticationProperties(providerName, redirectUrl);

            return new ChallengeResult(providerName, properties);
        }
    }
}