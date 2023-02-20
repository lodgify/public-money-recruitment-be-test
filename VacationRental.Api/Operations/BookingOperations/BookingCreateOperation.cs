using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Booking;
using Models.ViewModels.Rental;
using Mapster;

namespace VacationRental.Api.Operations.BookingOperations;

public sealed class BookingCreateOperation : IBookingCreateOperation
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;

    public BookingCreateOperation(IBookingRepository bokingRepository, IRentalRepository rentalRepository)
    {
        _bookingRepository = bokingRepository;
        _rentalRepository = rentalRepository;
    }

    public Task<ResourceIdViewModel> ExecuteAsync(BookingBindingViewModel model)
    {
        if (model.Nights <= 0)
            throw new ValidationException(ExceptionMessageConstants.NightsValidationError);

        return DoExecuteAsync(model);
    }

    /// <summary>
    /// Creates new booking.
    /// 1. Checks all existing bookings for selected day and count of units for rental.
    /// 2. If there is any avaliable unit - creates new booking for selected date and reserv unit's id, by saving it to BookingDto.
    /// </summary>
    /// <param name="model"></param>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="RentalNotAvailableExcepton"></exception>
    private async Task<ResourceIdViewModel> DoExecuteAsync(BookingBindingViewModel model)
    {
        if (!await _rentalRepository.IsExists(model.RentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var rentalTask = _rentalRepository.Get(model.RentalId);
        var bookingsTask = _bookingRepository.GetAll(model.RentalId, model.Start, model.Start.AddDays(model.Nights));

        await Task.WhenAll(rentalTask, bookingsTask);

        var rental = rentalTask.Result;
        var bookingsForRequestedTime = bookingsTask.Result;

        if (bookingsForRequestedTime.Count() >= rental.Units.Count)
            throw new RentalNotAvailableExcepton(ExceptionMessageConstants.RentalNotAvailable);

        var bookings = await _bookingRepository.GetAll();
        var key = new ResourceIdViewModel { Id = bookings.Count() + 1 };

        var bookedUnits = bookingsForRequestedTime.Select(x => x.Unit).ToList();
        var avaliableUnitId = rental.Units.First(_ => !bookedUnits.Contains(_));

        var bookingDto = model.Adapt<BookingDto>();

        bookingDto.Id = key.Id;
        bookingDto.PreparationTimeInDays = rental.PreparationTimeInDays;
        bookingDto.Unit = avaliableUnitId;

        await _bookingRepository.Create(key.Id, bookingDto);

        return key;
    }
}
