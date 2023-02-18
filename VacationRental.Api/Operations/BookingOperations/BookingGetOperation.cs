using Models.ViewModels;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.BookingOperations;

public sealed class BookingGetOperation : IBookingGetOperation
{
    private readonly IBookingRepository _bookingRepository;

    public BookingGetOperation(IBookingRepository bokingRepository)
    {
        _bookingRepository = bokingRepository;
    }

    public BookingViewModel ExecuteAsync(int bookingId)
    {
        if (bookingId <= 0)
            throw new ApplicationException("Wrong Id");

        return DoExecute(bookingId);
    }

    private BookingViewModel DoExecute(int bookingId)
    {
        if (!_bookingRepository.IsExists(bookingId))
            throw new ApplicationException("Booking not found");

        return _bookingRepository.Get(bookingId);
    }
}
