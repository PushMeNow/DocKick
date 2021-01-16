using System.Reflection;
using AutoMapper;
using DocKick.Authentication.Extensions;
using DocKick.Data.Extensions;
using DocKick.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocKick.Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                                 {
                                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocKick.Authentication v1");
                                 });
            }

            app.UseCors(config =>
                        {
                            config.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin();
                        });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var authSettings = Configuration.GetSection("Authentication")
                                            .Get<AuthSettings>();

            services.AddSingleton(authSettings);
            services.AddDatabaseConfigs(Configuration.GetConnectionString("DocKickAuthentication"));

            services.AddIdentityServerConfig(authSettings);

            services.AddSwaggerConfigs();
            services.AddDependencies(Configuration);
            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));
        }
    }
}