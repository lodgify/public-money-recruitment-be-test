using LanguageExt;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Interfaces
{
    public interface IBookingService
    {
        Result<BookingViewModel> GetBookingById(int bookingId);
        Result<ResourceIdViewModel> AddNewBooking(BookingBindingModel booking);
    }
}
