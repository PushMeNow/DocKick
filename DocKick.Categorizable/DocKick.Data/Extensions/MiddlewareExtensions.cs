using DocKick.Data.Repositories;
using DocKick.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocKick.Data.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddDatabaseConfig(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CategorizableDbContext>(config =>
                                                          {
                                                              config.UseSqlServer(connectionString);
                                                          });

            services.AddScoped<IBlobContainerRepository, BlobContainerRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();

            return services;
        }
    }
}