using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocKick.Data.Repositories;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Services
{
    public abstract class BaseDataService<TRepository, TEntity, TModel, TId> : IDataService<TModel, TId>
        where TEntity : class
        where TRepository : IRepository<TEntity>
        where TId : struct
    {
        protected readonly IMapper Mapper;
        protected readonly TRepository Repository;

        protected BaseDataService(TRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<TModel[]> GetAll()
        {
            return await Repository.GetAll()
                                   .ProjectTo<TModel>(Mapper.ConfigurationProvider)
                                   .ToArrayAsync();
        }

        public virtual async Task<TModel> GetById(TId id)
        {
            var entity = await Repository.GetById(id);

            ExceptionHelper.ThrowNotFoundIfEmpty(entity, typeof(TEntity).Name);

            var mapped = Map<TModel>(entity);

            return mapped;
        }

        public virtual async Task<TModel> Create(TModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(model, nameof(model));

            var entity = Map(model);

            await Repository.Create(entity);
            await Repository.Save();

            var mapped = Map(entity);

            return mapped;
        }

        public virtual async Task<TModel> Update(TId id, TModel request)
        {
            var entity = await Repository.GetById(id);

            ExceptionHelper.ThrowNotFoundIfEmpty(entity, typeof(TEntity).Name);

            entity = Map(request, entity);

            Repository.Update(entity);
            await Repository.Save();

            return Map(entity);
        }

        public virtual async Task Delete(TId id)
        {
            await Repository.Delete(id);
            await Repository.Save();
        }

        #region Maps

        protected TEntity Map(TModel request)
        {
            return Map<TEntity>(request);
        }

        protected TModel Map(TEntity entity)
        {
            return Map<TModel>(entity);
        }

        protected TEntity Map(TModel request, TEntity entity)
        {
            return Map<TModel, TEntity>(request, entity);
        }

        protected TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        protected TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #endregion
    }
}