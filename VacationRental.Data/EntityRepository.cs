using System.Linq;
using System.Linq.Expressions;

namespace VacationRental.Data
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDictionary<int, TEntity> _collection;

        public EntityRepository(IDictionary<int, TEntity> collection)
        {
            _collection = collection;
        }

        public TEntity Add(TEntity entity)
        {
            entity.Id = _collection.Count + 1;
            if (_collection.TryAdd(entity.Id, entity))
            {
                return entity;
            }
            else
            {
                throw new ApplicationException("An error occured while inserting the entity.");
            }
        }

        public bool Delete(int id)
        {
            return _collection.Remove(id);
        }

        public IList<TEntity> FindEntities(Expression<Func<TEntity, bool>> expression)
        {
            return _collection.Values.Where(expression.Compile()).ToList();
        }

        public IDictionary<int, TEntity> GetAllEntities()
        {
            return new Dictionary<int, TEntity>(_collection);
        }

        public TEntity GetEntityById(int id)
        {
            if (_collection.TryGetValue(id, out var entity))
            {
                return entity;
            }
            else
            {
                throw new ApplicationException($"{typeof(TEntity)} Not Found");
            }
        }

        public TEntity Update(TEntity entity)
        {
            _collection[entity.Id] = entity;
            return entity;
        }
    }
}
