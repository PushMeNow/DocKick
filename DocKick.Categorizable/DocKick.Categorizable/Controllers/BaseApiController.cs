using System;
using DocKick.Categorizable.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid UserId => User.GetUserId();
    }
}