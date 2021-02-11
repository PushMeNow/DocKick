using DocKick.Data.Repositories;
using DocKick.Entities.Blobs;
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
                                                              config.UseSqlServer(connectionString,
                                                                                  q =>
                                                                                  {
                                                                                      q.MigrationsAssembly("DocKick.Data");
                                                                                  });
                                                          });

            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Blob>, BlobRepository>();

            return services;
        }
    }
}