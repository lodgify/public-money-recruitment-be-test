using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;
using VacationRental.Infrastructure.Persist.PersistModels;

namespace VacationRental.Infrastructure.Persist
{
    public sealed class RentalRepository : IRentalRepository
    {
        private readonly Dictionary<int, RentalDataModel> _rentals = new Dictionary<int, RentalDataModel>();

        public Rental Get(RentalId id)
        {
            if (_rentals.TryGetValue(id.Id, out var rentalDataModel))
            {
                return MapToDomain(rentalDataModel);
            }

            throw new RentalNotFoundException(id);
        }

        public Rental Add(Rental rental)
        {
            var newRentalDataModel = MapToDataModel(rental);
            newRentalDataModel.Id = NextId(); // storage is responsible for generating new IDs. 

            _rentals.Add(newRentalDataModel.Id, newRentalDataModel);

            return MapToDomain(newRentalDataModel); // returns a domain object with the new ID.
        }

        private int NextId()
        {
            var maxId = _rentals.Keys.Max(id => id);
            return maxId + 1;
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
