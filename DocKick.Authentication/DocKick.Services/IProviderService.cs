using System.Collections.Generic;
using System.Threading.Tasks;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Authentication;

namespace DocKick.Services
{
    public interface IProviderService
    {
        Task<IReadOnlyCollection<AuthenticationProviderModel>> GetProviders();
        AuthenticationProperties GetAuthenticationProperties(string providerName, string redirectUrl);
    }
}