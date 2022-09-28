using System.Collections.Generic;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Services.Interfaces
{
    public interface IBookingsService
    {
        Booking GetBooking(int bookingId);
        List<Booking> GetBookings();
        int CreateBooking(CreateBooking booking);
    }
}
