using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class ProviderService : IProviderService
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProviderService(SignInManager<IdentityUser> signInManager)
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
    }
}