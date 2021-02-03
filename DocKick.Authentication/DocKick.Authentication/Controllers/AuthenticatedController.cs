using System;
using DocKick.Extensions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public abstract class AuthenticatedController : Controller
    {
        private Guid? _userId;
        
        protected Guid UserId => _userId ??= User.GetUserId();
    }
}