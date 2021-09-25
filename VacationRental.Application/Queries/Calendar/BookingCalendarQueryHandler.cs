using System;
using System.Collections.Generic;
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

            return new CalendarViewModel
            {
                RentalId = query.RentalId,
                Dates = FormRentalCalendarDates(bookings, query.Start, query.Nights)
            };
        }

        private List<CalendarDateViewModel> FormRentalCalendarDates(IReadOnlyCollection<Domain.Entities.Booking> bookings,  DateTime start, int nights)
        {
            var calendarDates = Enumerable.Range(0, nights)
                .Select(i => start.AddDays(i))
                .Select(date => (date, reserved: bookings.Where(booking => booking.WithinBookingPeriod(date)),
                    underPreparation: bookings.Where(booking => booking.WithinPreparationPeriod(date))))
                .Select(tuple => FormCalendarViewModel(tuple.date, tuple.reserved, tuple.underPreparation));

            return calendarDates.ToList();
        }

        private CalendarDateViewModel FormCalendarViewModel(DateTime date,
            IEnumerable<Domain.Entities.Booking> bookings, IEnumerable<Domain.Entities.Booking> bookingUnderPreparation)
        {
            return new CalendarDateViewModel
            {
                Date = date,
                Bookings = bookings.Select(booking => new CalendarBookingViewModel { Id = (int)booking.Id, Unit = booking.Unit }).ToList(),
                PreparationTimes = bookingUnderPreparation.Select(booking=> new PreparationTimesViewModel { Unit = booking.Unit }).ToList()
            };
        }
    }
}
