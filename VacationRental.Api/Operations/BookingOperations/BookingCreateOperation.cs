using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Repository;

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

    private async Task<ResourceIdViewModel> DoExecuteAsync(BookingBindingViewModel model)
    {
        if (!await _rentalRepository.IsExists(model.RentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var bookings = await _bookingRepository.GetAll();

        for (var i = 0; i < model.Nights; i++)
        {
            var count = 0;
            foreach (var booking in bookings)
            {
                if (booking.RentalId == model.RentalId
                    && (booking.Start <= model.Start.Date 
                        && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                    || (booking.Start < model.Start.AddDays(model.Nights) &&
                        booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                    || (booking.Start > model.Start &&
                        booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                {
                    count++;
                }
            }

            if (count >= (await _rentalRepository.Get(model.RentalId)).Units)
                throw new ApplicationException("Not available");
        }


        var key = new ResourceIdViewModel { Id = bookings.Count() + 1 };

        await _bookingRepository.Create(key.Id, new BookingViewModel
        {
            Id = key.Id,
            Nights = model.Nights,
            RentalId = model.RentalId,
            Start = model.Start.Date
        });

        return key;
    }
}
