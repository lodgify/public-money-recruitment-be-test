using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;

namespace VacationRental.Infrastructure.Persist
{
    public sealed class RentalRepository : IRentalRepository
    {
        private readonly Dictionary<RentalId, Rental> _rentals = new Dictionary<RentalId, Rental>();

        public Rental Get(RentalId id)
        {
            if (_rentals.TryGetValue(id, out var rental))
            {
                return rental;
            }

            throw new RentalNotFoundException(id);
        }
    }
}
