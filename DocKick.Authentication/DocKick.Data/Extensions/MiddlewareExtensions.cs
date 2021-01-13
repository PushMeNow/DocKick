using DocKick.Entities.Users;
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
                                                                                     builder.MigrationsAssembly(typeof(DocKickAuthDbContext).Assembly.GetName().Name);
                                                                                 });
                                                        });

            services.AddIdentity<User, Role>(q =>
                                           {
                                               q.Password = new PasswordOptions
                                                            {
                                                                RequireDigit = false,
                                                                RequiredLength = 4,
                                                                RequireLowercase = false,
                                                                RequireUppercase = false,
                                                                RequiredUniqueChars = 0,
                                                                RequireNonAlphanumeric = false
                                                            };
                                           })
            .AddEntityFrameworkStores<DocKickAuthDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}