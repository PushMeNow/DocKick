using DocKick.Services;
using DocKick.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocKick.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("Authentication")
                                            .Get<AuthSettings>();

            services.AddSingleton(authSettings);

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}