using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Infrastructure.Mapping
{
    public class ObjectMapper : IObjectMapper
    {
        public static ObjectMapper Global => new ObjectMapper();

        static ObjectMapper()
        {
            TypeAdapterConfig.GlobalSettings.Default.Settings.ValueAccessingStrategies
                       .Remove(ValueAccessingStrategy.GetMethod);
            TypeAdapterConfig.GlobalSettings.Default.Settings.ValueAccessingStrategies
                       .Remove(ValueAccessingStrategy.FlattenMember);
            TypeAdapterConfig.GlobalSettings.Default.Settings.ValueAccessingStrategies
                       .Remove(ValueAccessingStrategy.Dictionary);

            TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
        }

        public IList<IMapperRegister> Scan(params Assembly[] assemblies)
            => Scan(assemblies.ToList());

        public IList<IMapperRegister> Scan(IEnumerable<Assembly> assemblies)
        {
            var registers = assemblies
                .Select(assembly => assembly.GetTypes()
                      .Where(x => typeof(IMapperRegister).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo())
                                  && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(regTypes =>
                        regTypes.Select(type => (IMapperRegister)Activator.CreateInstance(type))).ToList();

            foreach (IMapperRegister register in registers)
                register.Register(this);

            return registers;
        }

        public MapConfig<TSource, TDestination> CreateMapConfig<TSource, TDestination>(int maxDepth = 1)
            => new MapConfig<TSource, TDestination>(maxDepth);

        public object Map<TSource>(TSource sourceInstance, Type destType)
            => sourceInstance.Adapt(sourceInstance.GetType(), destType);

        public TDestination Map<TSource, TDestination>(TSource sourceInstance)
           => sourceInstance.Adapt<TSource, TDestination>();

        public TDestination Map<TSource, TDestination>(TSource sourceInstance, TDestination destinationInstance)
            => sourceInstance.Adapt(destinationInstance);

        public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> sourceInstance)
            => sourceInstance.ProjectToType<TDestination>();

        public IList<TDestination> MapList<TSource, TDestination>(IList<TSource> sourceInstances)
            => Map<IList<TSource>, IList<TDestination>>(sourceInstances);

        public TDestination Map<TSource, TDestination>(TSource sourceInstance, Dictionary<string, object> parameters)
        {
            var toReturn = sourceInstance.BuildAdapter();
            foreach (var param in parameters)
                toReturn.AddParameters(param.Key, param.Value);
            return toReturn.AdaptToType<TDestination>();
        }

        public IList<TDestination> MapList<TSource, TDestination>(IList<TSource> sourceInstances, Dictionary<string, object> parameters)
            => Map<IList<TSource>, IList<TDestination>>(sourceInstances, parameters);

        public T Parameters<T>(string paramName) => (T)MapContext.Current.Parameters[paramName];

        public bool HasParameter(string paramName) => MapContext.Current != null && MapContext.Current.Parameters.ContainsKey(paramName);
    }
}
