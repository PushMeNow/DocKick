using System.Reflection;
using AutoMapper;
using Azure.Storage.Blobs;
using DocKick.Categorizable.Extensions;
using DocKick.Categorizable.Settings;
using DocKick.Data.Extensions;
using DocKick.Services.Blobs;
using DocKick.Services.Categories;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocKick.Categorizable
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocKick.Categorizable v1"));
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
                                 endpoints.MapControllers()
                                          .RequireAuthorization();
                             });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection("Authentication")
                                            .Get<AuthSettings>();

            services.AddSingleton(authSettings);

            services.AddControllers()
                    .AddFluentValidation(options =>
                                         {
                                             options.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                                             options.RegisterValidatorsFromAssembly(Assembly.Load("DocKick.Validation"));
                                         });

            services.AddSwaggerConfigs();

            services.AddDatabaseConfig(Configuration.GetConnectionString("DocKickCategorizable"));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                                                     {
                                                         options.Authority = authSettings.Authority;
                                                         options.SaveToken = true;
                                                     });

            services.AddAuthorization(options =>
                                      {
                                          options.AddPolicy("ApiScope",
                                                            policy =>
                                                            {
                                                                policy.RequireAuthenticatedUser();
                                                                policy.RequireClaim("scope", "api1");
                                                            });
                                      });

            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));

            services.AddScoped(_ => new BlobServiceClient(Configuration.GetConnectionString("AzureBlobStorage")));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBlobService, BlobService>();
        }
    }
}