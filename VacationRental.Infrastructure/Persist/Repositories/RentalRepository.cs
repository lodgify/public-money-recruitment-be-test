using System;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Infrastructure.Persist.Repositories
{
    public sealed class RentalRepository : IRentalRepository
    {
        private readonly IInMemoryDataStorage<RentalDataModel> _storage;

        public RentalRepository(IInMemoryDataStorage<RentalDataModel> storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public ValueTask<Rental> Get(RentalId id)
        {
            if (_storage.TryGetValue(id.Id, out var rentalDataModel))
            {
                return new ValueTask<Rental>(MapToDomain(rentalDataModel)); 
            }

            throw new RentalNotFoundException(id);
        }

        public ValueTask<Rental> Add(Rental rental)
        {
            var newRentalDataModel = MapToDataModel(rental);
            _storage.Add(newRentalDataModel);

            return new ValueTask<Rental>(MapToDomain(newRentalDataModel)); // returns a domain object with the new ID.
        }

        private static RentalDataModel MapToDataModel(Rental rental)
        {
            return new RentalDataModel
            {
                Id = rental.Id.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }

        private static Rental MapToDomain(RentalDataModel dataModel) =>
            new Rental(new RentalId(dataModel.Id), dataModel.Units, dataModel.PreparationTimeInDays);
    }
}
