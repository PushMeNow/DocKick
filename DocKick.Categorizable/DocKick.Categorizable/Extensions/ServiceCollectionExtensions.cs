using System;
using DocKick.Data.Repositories;
using DocKick.Entities.Categories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DocKick.Categorizable.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<Category>, CategoryRepository>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfigs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new OpenApiInfo
                                                    {
                                                        Title = "DocKick.Categorizable",
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

            return services;
        }
    }
}