using System;
using System.Security.Claims;

namespace DocKick.Categorizable.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var nameIdentifier = claimsPrincipal.GetNameIdentifier();

            return Guid.Parse(nameIdentifier);
        }

    }
}