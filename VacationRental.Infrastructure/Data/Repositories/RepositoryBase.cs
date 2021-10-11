using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Base;
using VacationRental.Domain.Interfaces;
using VacationRental.Infrastructure.DataBase;

namespace VacationRental.Infrastructure.Data.Repositories
{
    public class RepositoryBase<Entity> : IAsyncRepository<Entity> where Entity : BaseEntity
    {
        private Dictionary<Guid, IBaseEntity> _entityStore;
        private EFContext _dbContext;

        public RepositoryBase(EFContext dbContext)
        {
            _dbContext = dbContext;
            dbContext.getStore<Entity>(ref _entityStore);
        }

        public Task<Entity> AddAsync(Entity entity)
        {
            _entityStore.Add(entity.Guid, entity);

            return Task.FromResult(entity);
        }

        private bool IsValidEntityId(int id)
        {
            return _entityStore.Count > id && id > -1;
        }

        public Task<bool> DeleteAsync(Entity entity)
        {
            if (_entityStore.ContainsKey(entity.Guid))
            {
                _entityStore.Remove(entity.Guid);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public async Task<Entity> GetAsync(int Id)
        {
            var matchesId = await ListAsync((x) => x.Id == Id);
            Entity result = matchesId.FirstOrDefault();
            if (result == null)
            {
                throw new Exception("Id not Found");
            }
            return result;
        }
        public Task<Entity> GetAsync(Guid guid)
        {
            if (_entityStore.ContainsKey(guid))
            {
                return Task.FromResult((Entity)_entityStore[guid]);
            }
            throw new Exception("Id not found");
        }

        public Task<List<Entity>> ListAsync(Func<IBaseEntity, bool> expression)
        {
            var results = _entityStore.Values.Where(expression);
            return Task.FromResult(results.Select(x => (Entity)x).ToList());
        }

        public Task<Entity> UpdateAsync(Entity entity)
        {
            if (_entityStore.ContainsKey(entity.Guid))
            {
                _entityStore[entity.Guid] = entity;
                return Task.FromResult(entity);
            }
            throw new Exception("Not found Id");
        }
    }
}