using DocKick.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocKick.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(IServiceCollection services)
        {
            services.AddScoped<IProviderService, ProviderService>();

            return services;
        }
    }
}