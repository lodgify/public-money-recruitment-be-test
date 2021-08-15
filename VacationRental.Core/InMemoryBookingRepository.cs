using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Core
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, Booking> _bookings;

        public InMemoryBookingRepository(IDictionary<int, Booking> bookings)
        {
            _bookings = bookings;
        }

        public int Create(Booking booking)
        {
            booking.Id = _bookings.Keys.Count + 1;

            _bookings.Add(booking.Id, booking);

            return booking.Id;
        }

        public List<Booking> GetAll()
        {
            return _bookings.Values.ToList();
        }

        public Booking Get(int id)
        {
            if (_bookings.ContainsKey(id))
            {
                return _bookings[id];
            }

            return null;
        }
    }
}
