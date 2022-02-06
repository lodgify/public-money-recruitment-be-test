using System.Collections.Generic;
using Domain.DAL;
using Domain.DAL.Models;

namespace Infrastructure.DAL
{
    public class EntityRepository <TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDictionary<int, TEntity> _data;

        public EntityRepository(IDictionary<int, TEntity> data)
        {
            _data = data;
        }
        
        public IDictionary<int, TEntity> Query => _data;

        public int Insert(TEntity entity)
        {
            var key =_data.Keys.Count + 1;
            
            entity.Id = key;
            
            _data.Add(key, entity);

            return key;
        }

        public TEntity Find(int key)
        {
            return _data[key];
        }

        public void Update(TEntity entity)
        {
            var entityToUpdate = _data[entity.Id];

            _data.Remove(entity.Id);

            _data.Add(entityToUpdate.Id, entity);
        }
    }
}