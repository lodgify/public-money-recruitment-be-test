using Models.ViewModels.Booking;

namespace VacationRental.Api.Operations.BookingOperations;

public interface IBookingGetOperation
{
    Task<BookingViewModel> ExecuteAsync(int bookingId);
}
