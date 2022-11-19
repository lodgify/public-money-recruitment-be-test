using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;

namespace VacationRental.Infrastructure.Repositories
{
    internal class RentalRepository : IRentalRepository
    {
        private readonly IList<Rental> _rentals = new List<Rental>();

        public Rental Get(int id)
        {
            return _rentals.SingleOrDefault(x => x.Id == id);
        }

        public int Add(Rental rental)
        {
            var latest = _rentals.LastOrDefault();
            var newRentalId = latest is null ? 1 : latest.Id + 1;

            rental.SetRentalId(newRentalId);

            _rentals.Add(rental);

            return newRentalId;
        }
    }
}
