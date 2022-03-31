using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Bookings;

namespace VacationRental.Infrastructure.Persistence
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, BookingModel> _bookings = new Dictionary<int, BookingModel>();
        public int Save(BookingModel booking)
        {
            if (_bookings.ContainsKey(booking.Id))
                throw new ApplicationException("Booking exist");
            
            _bookings.Add(booking.Id, booking); 

            return booking.Id;
        }

        public BookingModel Get(int id)
        {
            if (!_bookings.ContainsKey(id))
                return null;

            return _bookings[id];
        }

        public int GetLastId()
        {
            return _bookings.Count + 1;
        }

        public ICollection<BookingModel> GetAll()
        {
            return _bookings.Values;
        }

        public IEnumerable<BookingModel> GetByRentalId(int id)
        {
            return _bookings.Values.Where(x => x.RentalId == id).ToList();
        }
    }
}