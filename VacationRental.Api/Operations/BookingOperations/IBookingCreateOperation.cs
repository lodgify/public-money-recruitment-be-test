using Models.ViewModels;

namespace VacationRental.Api.Operations.BookingOperations;

public interface IBookingCreateOperation
{
    Task<ResourceIdViewModel> ExecuteAsync(BookingBindingViewModel model);
}
