using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Queries.Calendar.ViewModel;
using VacationRental.Domain.Repositories.ReadOnly;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Calendar
{
    public class BookingCalendarQueryHandler : IRequestHandler<BookingCalendarForRentalQuery, CalendarViewModel>
    {
        private readonly IBookingReadOnlyRepository _bookingRepository;

        public BookingCalendarQueryHandler(IBookingReadOnlyRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public async Task<CalendarViewModel> Handle(BookingCalendarForRentalQuery query, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetByRentalId(new RentalId(query.RentalId));

            var calendarDates =  Enumerable.Range(0, query.Nights)
                .Select(i => query.Start.AddDays(i))
                .Select(date => (date, dateBookings: bookings.Where(booking => booking.WithinBookingPeriod(date))))
                .Select(tuple => new CalendarDateViewModel
                {
                    Date = tuple.date,
                    Bookings = tuple.dateBookings
                        .Select(booking => new CalendarBookingViewModel {Id = (int) booking.Id, Unit = booking.Unit}).ToList()
                }).ToList();
                

            return new CalendarViewModel
            {
                RentalId = query.RentalId,
                Dates = calendarDates
            };
        }
    }
}
