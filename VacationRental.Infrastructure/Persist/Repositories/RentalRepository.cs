using System;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Infrastructure.Persist.Repositories
{
    public sealed class RentalRepository : InMemoryRepository<Rental, RentalId, RentalDataModel>, IRentalRepository
    {

        public RentalRepository(IInMemoryDataStorage<RentalDataModel> storage) : base(storage)
        {

        }

        protected override int RetrieveId(RentalId entity) => entity.Id;

        protected override Exception GetNotFoundException(RentalId id) => new RentalNotFoundException(id);

        protected override Rental MapToDomain(RentalDataModel dataModel) =>
            new Rental(new RentalId(dataModel.Id), dataModel.Units, dataModel.PreparationTimeInDays);

        protected override RentalDataModel MapToDataModel(Rental rental)
        {
            return new RentalDataModel
            {
                Id = rental.Id.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }
    }
}
