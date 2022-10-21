using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface IBookingRepository
    {
        BookingViewModel GetBooking(int bookingId);
        ResourceIdViewModel InsertNewBooking(BookingBindingModel booking);
    }
}
