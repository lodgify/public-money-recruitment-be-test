using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Services.Dto;
using VacationRental.Services.Abstractions;
using VacationRental.Data.Model.Enums;

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
        };

        for (var i = 0; i < filter.Nights; i++)
        {
            var date = new CalendarDateDto
            {
                Date = filter.Start.Date.AddDays(i),
            };

            var bookings = _bookingRepository
                .GetAll()
                .Where(booking => booking.RentalId == filter.RentalId
                    && booking.Start <= date.Date
                    && booking.Start.AddDays(booking.Nights) > date.Date);

            date.Bookings.AddRange(bookings
                .Where(booking => booking.Type == BookingType.Booking)
                .Select(booking => new CalendarBookingDto { Id = booking.Id, Unit = booking.Unit }));

            date.PreparationTimes.AddRange(bookings
                .Where(booking => booking.Type == BookingType.Service)
                .Select(booking => new CalendarPreparationTimeDto { Unit = booking.Unit }));

            result.Dates.Add(date);
        }

        return result;
    }
}
