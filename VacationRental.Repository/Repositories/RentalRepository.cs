using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.Repository.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalEntity> _rentals;

        public RentalRepository(IDictionary<int, RentalEntity> rentals)
        {
            _rentals = rentals;
        }

        public RentalEntity GetRentalEntity(int rentalId)
        {
            RentalEntity rentalEntity;
            _rentals.TryGetValue(rentalId, out rentalEntity);

            return rentalEntity;
        }

        public int CreateRentalEntity(RentalEntity rentalEntity)
        {
            rentalEntity.Id = _rentals.Keys.Count + 1;
            _rentals.Add(rentalEntity.Id, rentalEntity);

            return rentalEntity.Id;
        }
    }
}
