using System.ComponentModel.DataAnnotations;
using Mapster;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Booking;

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

    /// <summary>
    /// Returnes existing booking.
    /// </summary>
    /// <param name="bookingId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private async Task<BookingViewModel> DoExecuteAsync(int bookingId)
    {
        if (!await _bookingRepository.IsExists(bookingId))
            throw new NotFoundException(ExceptionMessageConstants.BookingNotFound);

        var booking = await _bookingRepository.Get(bookingId);

        return booking.Adapt<BookingViewModel>();
    }
}
