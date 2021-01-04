using System;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DocKickAuthDbContext context) : base(context) { }

        public async Task<RefreshToken> GetByToken(string refreshToken)
        {
            return await GetAll()
                .FirstOrDefaultAsync(q => q.Token == refreshToken);
        }

        public async Task DeleteByToken(string refreshToken)
        {
            var entity = await GetByToken(refreshToken);

            ExceptionHelper.ThrowNotFoundIfNull(entity, "Refresh token");

            Set.Remove(entity);
        }

        public async Task ClearUserRefreshTokens(Guid userId)
        {
            var entities = await Set.Where(q => q.UserId == userId)
                                    .ToArrayAsync();

            if (!entities.Any())
            {
                return;
            }

            Set.RemoveRange(entities);
        }
    }
}