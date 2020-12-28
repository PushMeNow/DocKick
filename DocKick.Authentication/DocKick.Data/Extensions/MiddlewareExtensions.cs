using DocKick.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocKick.Data.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddDatabaseConfigs(this IServiceCollection services, string connString)
        {
            services.AddDbContext<DocKickAuthDbContext>(options =>
                                                        {
                                                            options.UseSqlServer(connString,
                                                                              builder =>
                                                                              {
                                                                                  builder.MigrationsAssembly(typeof(DocKickAuthDbContext).Namespace);
                                                                              });
                                                        });

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DocKickAuthDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}