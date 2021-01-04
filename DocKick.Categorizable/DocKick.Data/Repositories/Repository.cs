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

        public async Task<TEntity> GetById(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task Create(TEntity entity)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(entity, nameof(entity));

            await Set.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(entity, nameof(entity));

            Set.Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);

            ExceptionHelper.ThrowArgumentNullIfNull(entity, nameof(entity));

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
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            if (_disposed)
            {
                return;
            }

            _context.Dispose();

            _disposed = true;
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}