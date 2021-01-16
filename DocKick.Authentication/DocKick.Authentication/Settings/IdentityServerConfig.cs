using System;
using System.Collections.Generic;
using DocKick.Services.Settings;
using IdentityServer4;
using IdentityServer4.Models;

namespace DocKick.Authentication.Settings
{
    public static class IdentityServerConfig
    {
        private const string ApiScopeName = "api1";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
                   {
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile()
                   };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                   {
                       new(ApiScopeName, "My API", Array.Empty<string>())
                   };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new ApiScope[]
                   {
                       new(ApiScopeName, "My API")
                   };
        }

        public static Client[] GetClients(AuthSettings authSettings)
        {
            return new Client[]
                   {
                       new()
                       {
                           ClientId = "doc-kick-frontend",
                           ClientUri = authSettings.Authority,
                           RequireClientSecret = false,
                           AccessTokenLifetime = 3600,
                           IdentityTokenLifetime = 3600,
                           AllowedGrantTypes = GrantTypes.Implicit,
                           AllowAccessTokensViaBrowser = true,
                           AlwaysIncludeUserClaimsInIdToken = true,
                           AccessTokenType = AccessTokenType.Jwt,
                           PostLogoutRedirectUris = new[]
                                                    {
                                                        $"{authSettings.Authority}/logout-callback"
                                                    },
                           RedirectUris = new[]
                                          {
                                              $"{authSettings.Authority}/login-callback"
                                          },
                           AllowedCorsOrigins = new[]
                                                {
                                                    authSettings.Authority
                                                },
                           // scopes that client has access to
                           AllowedScopes =
                           {
                               IdentityServerConstants.StandardScopes.OpenId,
                               IdentityServerConstants.StandardScopes.Profile,
                               ApiScopeName
                           },
                           AllowOfflineAccess = true,
                           RequireConsent = false
                       }
                   };
        }
    }
}