using System;
using System.Linq;
using System.Security.Claims;
using DocKick.Exceptions;
using DocKick.Services.Constants;

namespace DocKick.Authentication.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirstValue(ClaimNames.UserId);

            ExceptionHelper.ThrowArgumentNullIfNull(userIdClaim, nameof(userIdClaim));

            return Guid.Parse(userIdClaim);
        }
    }
}