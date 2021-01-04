using System;
using System.Threading.Tasks;
using DocKick.Entities.Users;

namespace DocKick.Data.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>, IDisposable
    {
        Task<RefreshToken> GetByToken(string refreshToken);

        Task DeleteByToken(string refreshToken);

        Task ClearUserRefreshTokens(Guid userId);
    }
}