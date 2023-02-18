using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.CalendarOperations;

public sealed class CalendarGetOperation : ICalendarGetOperation
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;

    public CalendarGetOperation(IBookingRepository bokingRepository, IRentalRepository rentalRepository)
    {
        _bookingRepository = bokingRepository;
        _rentalRepository = rentalRepository;
    }

    public Task<CalendarViewModel> ExecuteAsync(int rentalId, DateTime start, int nights)
    {
        if (rentalId <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalIdValidationError); 
        if (nights <= 0)
            throw new ValidationException(ExceptionMessageConstants.NightsValidationError);

        return DoExecuteAsync(rentalId, start, nights);
    }

    private async Task<CalendarViewModel> DoExecuteAsync(int rentalId, DateTime start, int nights)
    {
        if (!await _rentalRepository.IsExists(rentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var result = new CalendarViewModel
        {
            RentalId = rentalId,
            Dates = new List<CalendarDateViewModel>()
        };

        var bookings = await _bookingRepository.GetAll();

        for (var i = 0; i < nights; i++)
        {
            var date = new CalendarDateViewModel
            {
                Date = start.Date.AddDays(i),
                Bookings = new List<CalendarBookingViewModel>()
            };

            foreach (var booking in bookings)
            {
                if (booking.RentalId == rentalId
                    && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                {
                    date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                }
            }

            result.Dates.Add(date);
        }

        return result;
    }
}
