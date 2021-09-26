using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Common;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Infrastructure.Persist.Repositories
{
    public abstract class InMemoryRepository<TDomainEntity, TIdentifier, TDataModel>
    where TDataModel : class, new()
    where TDomainEntity : Entity<TIdentifier>
    where TIdentifier : ValueObject
    {
        private readonly IInMemoryDataStorage<TDataModel> _storage;

        protected InMemoryRepository(IInMemoryDataStorage<TDataModel> storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public ValueTask<TDomainEntity> Add(TDomainEntity entity)
        {
            var newDataModel = MapToDataModel(entity);
            _storage.Add(newDataModel);

            return new ValueTask<TDomainEntity>(MapToDomain(newDataModel)); // returns a domain object with the new ID.
        }

        public Task Update(TDomainEntity entity)
        {
            if (_storage.TryGetValue(RetrieveId(entity.Id), out _))
            {
                var updateDataModel = MapToDataModel(entity);
                _storage.Update(updateDataModel);

                return Task.CompletedTask;
            }

            throw GetNotFoundException(entity.Id);
        }

        public ValueTask<TDomainEntity> Get(TIdentifier id)
        {
            if (_storage.TryGetValue(RetrieveId(id), out var rentalDataModel))
            {
                return new ValueTask<TDomainEntity>(MapToDomain(rentalDataModel));
            }

            throw GetNotFoundException(id);
        }

        protected ValueTask<IReadOnlyCollection<TDomainEntity>> Get(Func<TDataModel, bool> specification)
        {
            var models = _storage.Get(specification)
                .Select(MapToDomain)
                .ToList();

            return new ValueTask<IReadOnlyCollection<TDomainEntity>>(models);
        }

        protected async Task Update(IReadOnlyCollection<TDomainEntity> entities)
        {
            // As far as all entities should be updated at once we need to check if all of them are in the storage
            // in case of an attempt to update not stored entity, NotFoundException should be thrown
            var notStoredEntity = entities.FirstOrDefault(entity => _storage.TryGetValue(RetrieveId(entity.Id), out _) == false);
            if (notStoredEntity != null)
            {
                throw GetNotFoundException(notStoredEntity.Id); 
            }

            foreach (var entity in entities)
            {
                await Update(entity);
            }
        }


        protected abstract TDomainEntity MapToDomain(TDataModel dataModel);
        protected abstract TDataModel MapToDataModel(TDomainEntity entity);
        protected abstract int RetrieveId(TIdentifier id);
        protected abstract Exception GetNotFoundException(TIdentifier id);
    }
}
