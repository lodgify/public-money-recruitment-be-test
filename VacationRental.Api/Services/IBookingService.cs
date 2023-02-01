using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface IBookingService
{
    BookingViewModel Get(int bookingId);
    ResourceIdViewModel Add(BookingBindingModel model);
}