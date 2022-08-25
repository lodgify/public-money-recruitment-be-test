using System.Collections.Generic;
using VacationRental.Core.Domain.Bookings;
using VacationRental.Services.Models.Booking;
using VacationRental.Services.Models.Calendar;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Bookings
{
    public interface IBookingsService
    {
        BookingResponse Get(int bookingId);
        BookingEntity Book(CreateBookingRequest request);
        IEnumerable<OverlappedBookingViewModel> GetOverlappings(RentalViewModel rental);
    }
}
