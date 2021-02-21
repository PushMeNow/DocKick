using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocKick.Services
{
    public interface IDataService<TModel, in TId>
        where TId : struct
    {
        Task<TModel[]> GetAll();

        Task<TModel> GetById(TId id);

        Task<TModel> Create(TModel model);

        Task<TModel> Update(TId id, TModel request);

        Task Delete(TId id);
    }
}