using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Infrastructure.Models;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalRepository(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        public int Add(Rental rental)
        {
            int id = _rentals.Keys.Count + 1;

            _rentals.Add(id, new Rental
            {
                Id = id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            });

            return id;
        }

        public bool DeleteAll(Func<Rental, bool> predicate)
        {
            try
            {
                var rentals = GetAll(predicate);
                foreach (var rental in rentals)
                    _rentals.Remove(rental.Id);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Exists(int Id)
        {
            return _rentals.ContainsKey(Id);
        }

        public Rental Get(int id)
        {
            if (_rentals.ContainsKey(id))
                return _rentals[id];
            else
                return null;
        }

        public IEnumerable<Rental> GetAll(Func<Rental, bool> predicate)
        {
            var bookings = _rentals
                .Select(booking => booking.Value)
                .Where(predicate);

            return bookings;
        }

        public Rental Update(Rental rental)
        {
            var storedRental = _rentals[rental.Id];
            storedRental.Id = rental.Id;
            storedRental.Units = rental.Units;
            storedRental.PreparationTimeInDays = rental.PreparationTimeInDays;

            return storedRental;
        }
    }
}
