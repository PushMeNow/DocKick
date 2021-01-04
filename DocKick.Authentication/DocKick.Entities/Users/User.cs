using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}