using System;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly DbContext _context;

        private bool _disposed;

        protected DbSet<TEntity> Set { get; }

        protected Repository(DbContext context)
        {
            _context = context;
            Set = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return Set.AsNoTracking();
        }
        
        /// <summary>
        /// Method for lazy loading props
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllWithTracking()
        {
            return Set;
        }

        public async Task<TEntity> GetById<T>(T id)
            where T : struct
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(id, nameof(id));
            
            return await Set.FindAsync(id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(entity, nameof(entity));

            var entryEntity = await Set.AddAsync(entity);

            return entryEntity.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(entity, nameof(entity));

            var entryEntity = Set.Update(entity);

            return entryEntity.Entity;
        }

        public async Task Delete<T>(T id) where T : struct
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(id, nameof(id));
            
            var entity = await GetById(id);

            ExceptionHelper.ThrowParameterNullIfEmpty(entity, nameof(entity));

            Set.Remove(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            if (_disposed)
            {
                return;
            }

            _context?.Dispose();

            _disposed = true;
        }
    }
}