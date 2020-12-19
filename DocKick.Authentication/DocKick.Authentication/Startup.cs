using DocKick.Authentication.Extensions;
using DocKick.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DocKick.Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddGoogle(options =>
                               {
                                   options.ClientId = Configuration["Authentication:Google:ClientId"];
                                   options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                               })
                    .AddGitHub(options =>
                               {
                                   options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                                   options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                                   options.Scope.Add(Configuration["Authentication:GitHub:Scope"]);
                               });

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new OpenApiInfo
                                                    {
                                                        Title = "DocKick.Authentication",
                                                        Version = "v1"
                                                    });
                                   });

            services.AddDependencies();

            services.AddDatabaseConfigs(Configuration.GetConnectionString("DocKickAuthentication"));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocKick.Authentication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }
    }
}