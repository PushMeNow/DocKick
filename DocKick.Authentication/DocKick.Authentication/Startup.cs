using System.Reflection;
using AutoMapper;
using DocKick.Authentication.Extensions;
using DocKick.Data.Extensions;
using DocKick.Services.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocKick.Authentication
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var authSettings = Configuration.GetSection("Authentication")
                                            .Get<AuthSettings>();

            services.AddSingleton(authSettings);

            services.AddAuthentication(config =>
                                       {
                                           config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                           config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                    .AddJwtBearer(config =>
                                  {
                                      var defaultConfig = authSettings.Options.Value;

                                      config.RequireHttpsMetadata = defaultConfig.RequireHttpsMetadata;
                                      config.SaveToken = defaultConfig.SaveToken;

                                      config.TokenValidationParameters = defaultConfig.TokenValidationParameters;
                                  });

            services.AddSwaggerConfigs();
            services.AddDatabaseConfigs(Configuration.GetConnectionString("DocKickAuthentication"));
            services.AddDependencies(Configuration);
            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));
        }
    }
}