using System.Collections.Generic;
using VacationRental.Services.Models.Booking;

namespace VacationRental.Services.Bookings
{
    public interface IBookingsService
    {
        IEnumerable<BookingDto> GetBookings();

        BookingDto GetBookingById(int bookingId);

        BookingDto AddBooking(CreateBookingRequest request);

        BookingDto UpdateBooking(int bookingId, CreateBookingRequest request);

        bool DeleteBooking(int bookingId);
    }
}
