using DocKick.Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DockKick.Data.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddDatabaseConfigs(this IServiceCollection services, string connString)
        {
            services.AddDbContext<DockKickAuthDbContext>(options =>
                                                         {
                                                             options.UseNpgsql(connString);
                                                         });

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DockKickAuthDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}