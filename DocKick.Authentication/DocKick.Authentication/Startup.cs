using System.Reflection;
using AutoMapper;
using DocKick.Authentication.Extensions;
using DocKick.Authentication.Settings;
using DocKick.Data.Extensions;
using DocKick.Entities.Users;
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

            app.UseIdentityServer();
            app.UseAuthorization();

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

            services.AddIdentityServer(options =>
                                       {
                                           options.Events.RaiseErrorEvents = true;
                                           // options.Events.RaiseFailureEvents = true;
                                           options.Events.RaiseInformationEvents = true;
                                           options.Events.RaiseSuccessEvents = true;
                                           options.UserInteraction.LoginUrl = $"{authSettings.Authority}/auth/login";
                                           options.UserInteraction.LogoutUrl = $"{authSettings.Authority}/auth/logout";
                                           options.IssuerUri = authSettings.Authority;
                                       })
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                    .AddInMemoryClients(IdentityServerConfig.GetClients())
                    .AddAspNetIdentity<User>();

            services.AddAuthentication();
            services.AddAuthorization();

            // services.AddAuthentication(config =>
            //                            {
            //                                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //                                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //                                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //                            })
            //         .AddJwtBearer(config =>
            //                       {
            //                           var defaultConfig = authSettings.Options.Value;
            //
            //                           config.RequireHttpsMetadata = defaultConfig.RequireHttpsMetadata;
            //                           config.SaveToken = defaultConfig.SaveToken;
            //                           config.Audience = "api1";
            //                       });

            services.AddSwaggerConfigs();
            services.AddDependencies(Configuration);
            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));
        }
    }
}