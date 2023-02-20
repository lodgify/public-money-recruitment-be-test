using Models.ViewModels.Booking;
using Models.ViewModels.Rental;

namespace VacationRental.Api.Operations.BookingOperations;

public interface IBookingCreateOperation
{
    Task<ResourceIdViewModel> ExecuteAsync(BookingBindingViewModel model);
}
