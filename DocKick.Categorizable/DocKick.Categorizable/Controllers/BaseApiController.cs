using System;
using System.Linq;
using FluentValidation;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.GetSubjectId());

        protected void CheckValidation()
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException(string.Join("\r\n", ModelState.Values.SelectMany(q => q.Errors.Select(w => w.ErrorMessage))));
            }
        }
    }
}