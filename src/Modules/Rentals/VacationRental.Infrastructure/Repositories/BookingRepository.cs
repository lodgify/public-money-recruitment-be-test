using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;

namespace VacationRental.Infrastructure.Repositories
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly IList<Booking> _bookings = new List<Booking>();

        public int Add(Booking booking)
        {
            var latest = _bookings.LastOrDefault();
            var newBookingId = latest is null ? 1 : latest.Id + 1;

            booking.SetBookingId(newBookingId);

            _bookings.Add(booking);

            return newBookingId;
        }

        public Booking Get(int id)
        {
            return _bookings.SingleOrDefault(x => x.Id == id);
        }

        public IReadOnlyCollection<Booking> GetAll(int rentalId)
        {
            return _bookings.Where(x => x.RentalId == rentalId).ToList();
        }
    }
}
