using System;
using System.Text;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DocKick.Services.Settings
{
    public sealed class AuthSettings
    {
        public AuthSettings()
        {
            Options = GetJwtBearerOptions();
        }

        public string TokenSecret { get; set; }
        public int AccessTokenLifeTime { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string Authority { get; set; }

        public Client[] Clients { get; set; }

        public string MetadataAddress { get; set; }

        public Lazy<JwtBearerOptions> Options { get; }

        public ApiSettings[] Apis { get; set; }

        private Lazy<JwtBearerOptions> GetJwtBearerOptions()
        {
            return new(() => new JwtBearerOptions
                             {
                                 RequireHttpsMetadata = false,
                                 SaveToken = true,
                                 Authority = Authority,
                                 TokenValidationParameters = new TokenValidationParameters
                                                             {
                                                                 ValidateIssuerSigningKey = false,
                                                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSecret)),
                                                                 ValidateIssuer = false,
                                                                 ValidateAudience = false
                                                             }
                             });
        }
    }
}