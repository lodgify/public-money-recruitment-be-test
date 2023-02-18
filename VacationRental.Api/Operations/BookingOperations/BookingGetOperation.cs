using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.BookingOperations;

public sealed class BookingGetOperation : IBookingGetOperation
{
    private readonly IBookingRepository _bookingRepository;

    public BookingGetOperation(IBookingRepository bokingRepository)
    {
        _bookingRepository = bokingRepository;
    }

    public Task<BookingViewModel> ExecuteAsync(int bookingId)
    {
        if (bookingId <= 0)
            throw new ValidationException(ExceptionMessageConstants.BookingBindingIdValidationError);

        return DoExecuteAsync(bookingId);
    }

    private async Task<BookingViewModel> DoExecuteAsync(int bookingId)
    {
        if (!await _bookingRepository.IsExists(bookingId))
            throw new NotFoundException(ExceptionMessageConstants.BookingNotFound);

        return await _bookingRepository.Get(bookingId);
    }
}
