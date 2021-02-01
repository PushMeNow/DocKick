using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocKick.Services
{
    public interface IDataService<TModel, in TRequest, in TId>
        where TId : struct
    {
        Task<TModel[]> GetAll();

        Task<TModel> GetById(TId id);

        Task<TModel> Create(TRequest request);

        Task<TModel> Update(TId id, TRequest request);

        Task Delete(TId id);
    }
}