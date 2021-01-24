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
    }
}