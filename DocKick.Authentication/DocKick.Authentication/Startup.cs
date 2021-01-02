using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DocKick.Authentication.Extensions;
using DocKick.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

            services.AddAuthentication(config =>
                                       {
                                           config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                           config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                    .AddJwtBearer(config =>
                                  {
                                      config.RequireHttpsMetadata = false;
                                      config.SaveToken = true;

                                      config.TokenValidationParameters = new TokenValidationParameters
                                                                         {
                                                                             ValidateIssuerSigningKey = false,
                                                                             IssuerSigningKey =
                                                                                 new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration
                                                                                                              ["Authentication:TokenSecret"])),
                                                                             ValidateIssuer = false,
                                                                             ValidateAudience = false,
                                                                             NameClaimType = ClaimTypes.Email
                                                                         };
                                  });

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new OpenApiInfo
                                                    {
                                                        Title = "DocKick.Authentication",
                                                        Version = "v1"
                                                    });

                                       c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                                                               new OpenApiSecurityScheme
                                                               {
                                                                   In = ParameterLocation.Header,
                                                                   BearerFormat = "JWT",
                                                                   Description = "Enter jwt.",
                                                                   Scheme = JwtBearerDefaults.AuthenticationScheme,
                                                                   Type = SecuritySchemeType.Http,
                                                                   Name = "Authorization"
                                                               });

                                       c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                {
                                                                    {
                                                                        new OpenApiSecurityScheme
                                                                        {
                                                                            Reference = new OpenApiReference
                                                                                        {
                                                                                            Type = ReferenceType.SecurityScheme,
                                                                                            Id = JwtBearerDefaults.AuthenticationScheme
                                                                                        }
                                                                        },
                                                                        Array.Empty<string>()
                                                                    }
                                                                });
                                   });

            services.AddDatabaseConfigs(Configuration.GetConnectionString("DocKickAuthentication"));
            services.AddDependencies(Configuration);
            services.AddAutoMapper(Assembly.Load("DocKick.Mapper"));
        }
    }
}