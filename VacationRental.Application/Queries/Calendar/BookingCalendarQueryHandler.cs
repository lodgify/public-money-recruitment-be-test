using System;
using System.Collections.Generic;
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
            var bookings = _bookingRepository.GetByRentalId(new RentalId(query.RentalId));
            var rentalCalendar = new CalendarViewModel
            {
                RentalId = query.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < query.Nights; i++)
            {
                var calendarDate = new CalendarDateViewModel
                {
                    Date = query.Start.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in bookings)
                {
                    if(booking.Within(calendarDate.Date))
                    {
                        calendarDate.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id.Id });
                    }
                }

                rentalCalendar.Dates.Add(calendarDate);
            }

            await Task.Delay(1);
            return rentalCalendar;
        }
    }
}
