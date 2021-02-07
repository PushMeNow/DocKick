using DocKick.Entities.Categories;

namespace DocKick.Data.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(CategorizableDbContext context) : base(context) { }
    }
}