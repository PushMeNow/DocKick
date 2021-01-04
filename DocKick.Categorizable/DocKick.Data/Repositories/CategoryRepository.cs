using DocKick.Entities.Category;

namespace DocKick.Data.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(CategorizableDbContext context) : base(context) { }
    }
}