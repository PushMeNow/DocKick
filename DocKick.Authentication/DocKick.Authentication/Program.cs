using System;
using System.Threading.Tasks;
using DocKick.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocKick.Authentication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();
#if DOCKER
            await EnsureDatabaseCreatedAsync(host);
#endif

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                                                 {
                                                     webBuilder.UseStartup<Startup>()
                                                               .UseWebRoot("wwwroot");

#if DOCKER
                                                     webBuilder.ConfigureAppConfiguration(builder =>
                                                                                          {
                                                                                              builder.AddJsonFile("appsettings.Docker.json");
                                                                                          });
#endif
                                                 });
        }

        private static async Task EnsureDatabaseCreatedAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DocKickAuthDbContext>();

                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex.Message, "An error occurred seeding the DB.");
            }
        }
    }
}