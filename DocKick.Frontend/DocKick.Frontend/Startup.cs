using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocKick.Frontend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(config =>
                                       {
                                           config.RootPath = "wwwroot/build";
                                       });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
                       {
                           spa.Options.SourcePath = "wwwroot";

                           if (env.IsDevelopment())
                           {
                               spa.UseReactDevelopmentServer("start");
                           }
                       });
        }
    }
}