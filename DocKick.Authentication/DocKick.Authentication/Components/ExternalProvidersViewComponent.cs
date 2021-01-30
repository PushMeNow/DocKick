using System.Linq;
using System.Threading.Tasks;
using DocKick.Entities.Users;
using DocKick.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Components
{
    public class ExternalProvidersViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;

        public ExternalProvidersViewComponent(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string returnUrl)
        {
            var externalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).Select(q => q.Name)
                                                                                                  .ToArray();

            ViewBag.ReturnUrl = returnUrl;

            return View("_ExternalProviders", externalProviders);
        }
    }
}