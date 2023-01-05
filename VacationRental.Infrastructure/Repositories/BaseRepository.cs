using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Primitives;

namespace VacationRental.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseDomainModel
    {
        public readonly IDictionary<int, T> _persistence;

        public BaseRepository()
        {
            if (this._persistence == null) 
            { 
                _persistence = new Dictionary<int, T>();
            }
        }

        public T Add(T entity)
        {
            entity.Id = _persistence.Keys.Count + 1;            
            _persistence.Add(entity.Id, entity);

            return _persistence[entity.Id];
        }

        public void Delete(T entity)
        {
            if (_persistence.ContainsKey(entity.Id))
                _persistence.Remove(entity.Id);
        }

        public IReadOnlyList<T> GetAll()
        {
            return _persistence.Values.ToList();
        }

        public T? GetById(int id)
        {
            if (!_persistence.ContainsKey(id))
                return null;
            
            return _persistence[id];
        }

        public T Update(T entity)
        {
            if (!_persistence.ContainsKey(entity.Id))
                throw new ApplicationException("object doesn't exists in persitence");
            
            _persistence[entity.Id] = entity;
            return entity;
        }
    }
}
