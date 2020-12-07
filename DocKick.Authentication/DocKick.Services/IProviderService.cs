using System.Collections.Generic;
using System.Threading.Tasks;
using DocKick.Services.Models;

namespace DocKick.Services
{
    public interface IProviderService
    {
        Task<IReadOnlyCollection<AuthenticationProviderModel>> GetProviders();
    }
}