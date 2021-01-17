using System.Security.Claims;

namespace DocKick.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        }
    }
}