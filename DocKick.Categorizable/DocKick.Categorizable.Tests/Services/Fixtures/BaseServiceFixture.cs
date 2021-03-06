﻿using System;
using AutoMapper;
using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data;

namespace DocKick.Categorizable.Tests.Services.Fixtures
{
    public abstract class BaseServiceFixture<TService> : IDisposable
    {
        public CategorizableDbContext Context { get; }

        public IMapper Mapper => MapperHelper.Instance;

        protected BaseServiceFixture(bool createContext = true)
        {
            if (!createContext)
            {
                return;
            }

            Context = InMemoryContextBuilder.CreateContext();

            InitDatabase();

            Context.SaveChanges();
        }

        public abstract TService CreateService();

        protected virtual void InitDatabase() { }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}