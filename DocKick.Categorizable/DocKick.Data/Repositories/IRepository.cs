﻿using System;
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

        Task<TEntity> Create(TEntity entity);

        TEntity Update(TEntity entity);

        Task Delete<T>(T id) where T : struct;

        Task Save();

        IQueryable<TEntity> GetAllWithTracking();

        Task ExecuteInTransaction(Action action);

        Task ExecuteInTransaction(Func<Task> func);

        void Delete(TEntity entity);
    }
}