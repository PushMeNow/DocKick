using System.Collections.Generic;
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
                       new IdentityResources.Profile(),
                       new IdentityResources.Email()
                   };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                   {
                       new(ApiScopeName, "My API")
                   };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
                   {
                       new()
                       {
                           ClientId = "client",
                           // ClientName = "DocKick",
                           ClientUri = "https://localhost:5001",
                           RequireClientSecret = false,
                           AccessTokenLifetime = 3600,
                           IdentityTokenLifetime = 3600,
                           AllowedGrantTypes = GrantTypes.Implicit,
                           AllowAccessTokensViaBrowser = true,
                           AlwaysIncludeUserClaimsInIdToken = true,
                           PostLogoutRedirectUris = new[]
                                                    {
                                                        "https://localhost:5001/logout-callback"
                                                    },
                           RedirectUris = new[]
                                          {
                                              "https://localhost:5001/login-callback"
                                          },
                           AllowedCorsOrigins = new[]
                                                {
                                                    "https://localhost:5001"
                                                },
                           // scopes that client has access to
                           AllowedScopes =
                           {
                               IdentityServerConstants.StandardScopes.OpenId,
                               IdentityServerConstants.StandardScopes.Profile,
                               ApiScopeName
                           },
                           AllowOfflineAccess = false,
                           RequireConsent = false
                       }
                   };
        }
    }
}