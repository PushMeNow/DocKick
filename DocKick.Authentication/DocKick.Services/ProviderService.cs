using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Data.Entities.Users;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class ProviderService : IProviderService
    {
        private readonly SignInManager<User> _signInManager;

        public ProviderService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IReadOnlyCollection<AuthenticationProviderModel>> GetProviders()
        {
            var providers = await _signInManager.GetExternalAuthenticationSchemesAsync();

            return providers.Select(q => new AuthenticationProviderModel
                                         {
                                             Name = q.Name,
                                             DisplayName = q.DisplayName
                                         })
                            .ToArray();
        }

        public AuthenticationProperties GetAuthenticationProperties(string providerName, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(providerName, redirectUrl);
        }
    }
}