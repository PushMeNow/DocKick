using System.Collections.Generic;
using System.Threading.Tasks;
using DocKick.Services;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<IEnumerable<AuthenticationProviderModel>> GetProviders()
        {
            return await _providerService.GetProviders();
        }
        
        public IActionResult ExternalLogin(string providerName)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _providerService.GetAuthenticationProperties(providerName, redirectUrl);

            return new ChallengeResult(providerName, properties);
        }
    }
}