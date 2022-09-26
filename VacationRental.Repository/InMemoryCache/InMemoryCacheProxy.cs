//using System.Diagnostics.CodeAnalysis;
//using Microsoft.Extensions.Caching.Memory;
//using VacationRental.Repository.Entities.Interfaces;
//using VacationRental.Repository.InMemoryCache.Interfaces;

//namespace VacationRental.Repository.InMemoryCache
//{
//    /// <summary>
//    /// A very simple inmemory cache - use it instead of Dictionary like it was initially in controllers
//    /// </summary>
//    [ExcludeFromCodeCoverage]
//    public class InMemoryCacheProxy : IInMemoryCacheProxy
//    {
//        private readonly IMemoryCache _memoryCache;

//        public InMemoryCacheProxy(IMemoryCache memoryCache)
//        {
//            _memoryCache = memoryCache;
//        }

//        public void AddOrUpdateEntity<T>(T entity)
//            where T : class, IEntity
//        {
//            _memoryCache.Set(entity.Id, entity);
//        }

//        public T GetEntity<T>(int entityId)
//            where T : class, IEntity
//        {
//            _memoryCache.TryGetValue(entityId, out T existingEntity);

//            return existingEntity;
//        }
//    }
//}
