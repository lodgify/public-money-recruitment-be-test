using VacationRental.Domain.Bookings;
using VacationRental.Infrastructure.DTOs;

namespace VacationRental.Infrastructure.Services.Interfaces
{
    public interface IBookingService
    {
        int CreateBooking(BookingsCreateInputDTO inputDTO);

        Booking GetBooking(int id);
    }
}
