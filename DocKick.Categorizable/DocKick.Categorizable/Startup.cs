using System.Reflection;
using AutoMapper;
using DocKick.Categorizable.Extensions;
using DocKick.Categorizable.Settings;
using DocKick.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocKick.Categorizable
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocKick.Categorizable v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection("Authentication")
                                            .Get<AuthSettings>();

            services.AddSingleton(authSettings);

            services.AddControllers();

            services.AddSwaggerConfigs();

            services.AddDatabaseConfig(Configuration.GetConnectionString("DocKickCategorizable"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddOpenIdConnect(config =>
                                      {
                                          config.Configuration.AuthorizationEndpoint = authSettings.AuthEndpoint;
                                          config.Configuration.TokenEndpoint = authSettings.TokenEndpoint;
                                          config.SaveTokens = true;
                                      });

            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));
        }
    }
}