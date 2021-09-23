using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;

namespace VacationRental.Infrastructure.Persist
{
    public sealed class BookingRepository : IBookingRepository
    {
        private readonly Dictionary<BookingId, Booking> _bookings = new Dictionary<BookingId, Booking>();

        public Booking Get(BookingId bookingId)
        {
            if (_bookings.TryGetValue(bookingId, out var booking))
            {
                return booking;
            }

            throw new BookingNotFoundException(bookingId);
        }
    }
}
