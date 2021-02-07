using System;
using System.Threading.Tasks;
using DocKick.Entities.Blobs;

namespace DocKick.Data.Repositories
{
    public interface IBlobContainerRepository : IRepository<BlobContainer>
    {
        Task<BlobContainer> GetByUserId(Guid userId);
    }
}