using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IBookingRepository
    {
        int BookingCount();
        BookingViewModel GetBooking(int id);
        
        void CreateBooking(BookingViewModel booking);
    }
}