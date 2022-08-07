using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingService
    {
        BookingViewModel GetBooking(int bookingId);
        ResourceIdViewModel AddBooking(BookingBindingModel model);
    }
}
