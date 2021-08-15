using System.Collections.Generic;

namespace VacationRental.Core
{
    public class InMemoryRentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals;

        public InMemoryRentalRepository(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        public int Create(Rental rental)
        {
            rental.Id = _rentals.Keys.Count + 1;

            _rentals.Add(rental.Id, rental);

            return rental.Id;
        }

        public Rental Get(int id)
        {
            if (_rentals.ContainsKey(id))
            {
                return _rentals[id];
            }

            return null;
        }
    }
}
