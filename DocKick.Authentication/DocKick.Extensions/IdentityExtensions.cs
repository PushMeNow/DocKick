using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        }
        
        public static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var nameIdentifier = claimsPrincipal.GetNameIdentifier();

            return Guid.Parse(nameIdentifier);
        }

        public static string CombineErrors(this IdentityResult identityResult)
        {
            return string.Join("\r\n", identityResult.Errors.Select(q => q.Description));
        }
    }
}