using Models.ViewModels;

namespace VacationRental.Api.Operations.BookingOperations;

public interface IBookingGetOperation
{
    BookingViewModel ExecuteAsync(int bookingId);
}
