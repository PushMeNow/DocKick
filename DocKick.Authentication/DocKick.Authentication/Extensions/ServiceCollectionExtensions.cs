using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using DocKick.Authentication.Settings;
using DocKick.Entities.Users;
using DocKick.Services;
using DocKick.Services.Settings;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DocKick.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfigs(this IServiceCollection services)
        {
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

            return services;
        }

        public static void AddIdentityServerConfig(this IServiceCollection services, AuthSettings authSettings)
        {
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
                    .AddJwtBearerClientAuthentication()
                    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                    .AddInMemoryApiScopes(IdentityServerConfig.GetScopes())
                    .AddInMemoryClients(authSettings.Clients)
                    .AddAspNetIdentity<User>();

            services.AddAuthentication()
                    .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                                                     options =>
                                                     {
                                                         options.Authority = authSettings.Authority;
                                                         options.SaveToken = true;
                                                         options.RequireHttpsMetadata = false;
                                                         options.MetadataAddress = authSettings.MetadataAddress;

                                                         options.TokenValidationParameters = new TokenValidationParameters()
                                                                                             {
                                                                                                 ValidateAudience = false,
                                                                                                 ValidateIssuerSigningKey = false,
                                                                                                 ValidateIssuer = false,
                                                                                                 NameClaimType = "name",
                                                                                                 RoleClaimType = "role"
                                                                                             };
                                                     },
                                                     null)
                    // .AddIdentityServerAuthentication(options =>
                    //                                  {
                    //                                      options.Authority = authSettings.Authority;
                    //                                      options.SaveToken = true;
                    //                                      options.SupportedTokens = SupportedTokens.Both;
                    //                                      options.RequireHttpsMetadata = false;
                    //                                  })
                    .AddGoogle(options =>
                               {
                                   options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                                   options.ClientId = "734351612309-0bl0o4vlsmfooue95ellut0fc833scmt.apps.googleusercontent.com";
                                   options.ClientSecret = "R0p2vUf9g8B5J7Ic-026U6YH";
                               });

            services.AddAuthorization();
        }
    }
}