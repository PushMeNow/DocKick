using DocKick.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocKick.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // services.AddScoped<IProviderService, ProviderService>();
            // services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}