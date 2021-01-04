using System;

namespace DocKick.Entities.Users
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public virtual User User { get; set; }

        public void UpdateExpiration(int expiration)
        {
            Expiration = DateTime.UtcNow.AddDays(expiration);
        }
    }
}