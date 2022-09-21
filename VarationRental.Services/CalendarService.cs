using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Services.Dto;
using VacationRental.Services.Abstractions;

namespace VacationRental.Services;

public class CalendarService : ICalendarService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;

    public CalendarService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
    {
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }

    public CalendarDto Get(CalendarFilterDto filter)
    {
        var rental = _rentalRepository.Get(filter.RentalId);

        var result = new CalendarDto
        {
            RentalId = filter.RentalId,
            Dates = new List<CalendarDateDto>()
        };

        for (var i = 0; i < filter.Nights; i++)
        {
            var date = new CalendarDateDto
            {
                Date = filter.Start.Date.AddDays(i),
                Bookings = new List<CalendarBookingDto>()
            };

            date.Bookings.AddRange(_bookingRepository
                .GetAll()
                .Where(booking => booking.RentalId == filter.RentalId
                    && booking.Start <= date.Date
                    && booking.Start.AddDays(booking.Nights) > date.Date)
                .Select(booking => new CalendarBookingDto { Id = booking.Id }));

            result.Dates.Add(date);
        }

        return result;
    }
}
