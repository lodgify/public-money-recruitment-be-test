using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IBookingRepository
    {
        int BookingCount();
        BookingViewModel GetBooking(int id);
        
        int CreateBooking(BookingViewModel booking);
    }
}