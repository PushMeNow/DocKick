using System;
using System.Threading.Tasks;
using DocKick.Entities.Blobs;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data.Repositories
{
    public class BlobContainerRepository : Repository<BlobContainer>, IBlobContainerRepository
    {
        public BlobContainerRepository(CategorizableDbContext context) : base(context)
        {
        }

        public Task<BlobContainer> GetByUserId(Guid userId)
        {
            return Set.FirstOrDefaultAsync(q => q.UserId == userId);
        }
    }
}