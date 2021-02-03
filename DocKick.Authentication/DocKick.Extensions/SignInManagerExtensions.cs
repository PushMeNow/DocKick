using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Extensions
{
    public static class SignInManagerExtensions
    {
        private const string LoginProviderKey = "LoginProvider";

        public static async Task<ExternalLoginInfo> GetExternalLoginInfoIdentityServer<TUser>(this SignInManager<TUser> signInManager)
            where TUser : class
        {
            var auth = await signInManager.Context.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var items = auth?.Properties?.Items;

            if (auth?.Principal == null
                || items == null
                || !items.ContainsKey(LoginProviderKey))
            {
                return null;
            }

            var providerKey = auth.Principal.GetNameIdentifier();
            var provider = items[LoginProviderKey];

            if (providerKey == null
                || provider == null)
            {
                return null;
            }

            var externalSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();

            var providerDisplayName = externalSchemes.FirstOrDefault(p => p.Name == provider)?.DisplayName ?? provider;

            return new ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
                   {
                       AuthenticationTokens = auth.Properties.GetTokens(),
                       AuthenticationProperties = auth.Properties
                   };
        }
    }
}