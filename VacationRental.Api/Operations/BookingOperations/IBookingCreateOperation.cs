using Models.ViewModels;

namespace VacationRental.Api.Operations.BookingOperations
{
    public interface IBookingCreateOperation
    {
        ResourceIdViewModel ExecuteAsync(BookingBindingViewModel model);
    }
}
