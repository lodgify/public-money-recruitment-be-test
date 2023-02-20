using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Calendar;
using Mapster;

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

    /// <summary>
    /// Returns Calendar for selected period (start date : start date + nigths)
    /// </summary>
    /// <param name="rentalId"></param>
    /// <param name="calendarStartDate"></param>
    /// <param name="nights"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private async Task<CalendarViewModel> DoExecuteAsync(int rentalId, DateTime calendarStartDate, int nights)
    {
        if (!await _rentalRepository.IsExists(rentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var result = new CalendarViewModel(rentalId, new List<CalendarDateViewModel>());

        var bookingsForSelectedPeriod = await _bookingRepository.GetAll(rentalId, calendarStartDate, calendarStartDate.AddDays(nights));

        for (var i = 0; i < nights; i++)
        {
            var currentCalendarDate = calendarStartDate.AddDays(i);
            var calendarDateViewModel = new CalendarDateViewModel(currentCalendarDate, new List<CalendarBookingViewModel>(), new List<CalendarUnitViewModel>());

            var listOfUsedUnits = bookingsForSelectedPeriod.Where(_ => CheckBookingTime(_, currentCalendarDate)).ToList();
            var listOfBeingPreparedUnits = bookingsForSelectedPeriod.Where(_ => CheckBookingPreparationTime(_, currentCalendarDate)).ToList();

            foreach (var unit in listOfUsedUnits)
            {
                calendarDateViewModel.Bookings.Add(unit.Adapt<CalendarBookingViewModel>());
            }

            foreach (var unit in listOfBeingPreparedUnits)
            {
                calendarDateViewModel.PreparationTimes.Add(unit.Adapt<CalendarUnitViewModel>());
            }

            result.Dates.Add(calendarDateViewModel);
        }

        return result;
    }

    private bool CheckBookingTime(BookingDto entity, DateTime date) =>
        entity.Start <= date
        && entity.Start.AddDays(entity.Nights) > date;

    private bool CheckBookingPreparationTime(BookingDto entity, DateTime date) =>
        entity.Start.AddDays(entity.Nights) <= date
        && entity.Start.AddDays(entity.Nights + entity.PreparationTimeInDays) > date;
}
