using DocKick.Entities.Blobs;

namespace DocKick.Data.Repositories
{
    public class BlobRepository : Repository<Blob>
    {
        public BlobRepository(CategorizableDbContext context) : base(context)
        {
        }
    }
}