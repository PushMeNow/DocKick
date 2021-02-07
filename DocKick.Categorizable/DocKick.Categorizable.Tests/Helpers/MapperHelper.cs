using System;
using System.Reflection;
using AutoMapper;

namespace DocKick.Categorizable.Tests.Helpers
{
    public static class MapperHelper
    {
        private static readonly Lazy<IMapper> _mapper = new(() =>
                                                            {
                                                                var config = new MapperConfiguration(q =>
                                                                                                     {
                                                                                                         q.AddMaps(Assembly.Load("DocKick.Mapper"));
                                                                                                     });

                                                                return config.CreateMapper();
                                                            });

        public static IMapper Instance => _mapper.Value;
    }
}