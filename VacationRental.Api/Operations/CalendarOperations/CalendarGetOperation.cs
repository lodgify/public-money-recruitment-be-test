using Models.ViewModels;
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

    public CalendarViewModel ExecuteAsync(int rentalId, DateTime start, int nights)
    {
        if (rentalId <= 0)
            throw new ApplicationException("Wrong Id"); 
        if (nights <= 0)
            throw new ApplicationException("Wrong nights");

        return DoExecute(rentalId, start, nights);
    }

    private CalendarViewModel DoExecute(int rentalId, DateTime start, int nights)
    {
        if (!_rentalRepository.IsExists(rentalId))
            throw new ApplicationException("Rental not found");

        var result = new CalendarViewModel
        {
            RentalId = rentalId,
            Dates = new List<CalendarDateViewModel>()
        };

        var bookings = _bookingRepository.GetAll();

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
