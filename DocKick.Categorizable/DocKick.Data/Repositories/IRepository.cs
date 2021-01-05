using System.Linq;
using System.Threading.Tasks;

namespace DocKick.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById<T>(T id)
            where T : struct;

        Task Create(TEntity entity);

        void Update(TEntity entity);

        Task Delete<T>(T id) where T : struct;

        Task Save();
    }
}