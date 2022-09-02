using System;
using System.Collections.Generic;
using System.Linq;

namespace VR.Infrastructure.Mapping.Interfaces
{
    public interface IObjectMapper
    {
        MapConfig<TSource, TDestination> CreateMapConfig<TSource, TDestination>(int maxDepth = 1);

        TDestination Map<TSource, TDestination>(TSource sourceInstance);

        object Map<TSource>(TSource sourceInstance, Type destType);

        TDestination Map<TSource, TDestination>(TSource sourceInstance, TDestination destinationInstance);

        TDestination Map<TSource, TDestination>(TSource sourceInstance, Dictionary<string, object> parameters);

        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> sourceInstance);

        IList<TDestination> MapList<TSource, TDestination>(IList<TSource> sourceInstances);

        IList<TDestination> MapList<TSource, TDestination>(IList<TSource> sourceInstances,
                                                           Dictionary<string, object> parameters);

        T Parameters<T>(string paramName);

        bool HasParameter(string paramName);
    }
}
