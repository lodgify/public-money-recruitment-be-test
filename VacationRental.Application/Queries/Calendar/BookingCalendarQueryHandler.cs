using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Application.Queries.Calendar.ViewModel;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Calendar
{
    public class BookingCalendarQueryHandler
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingCalendarQueryHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public async Task<CalendarViewModel> Handle(BookingCalendarForRentalQuery query)
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
                    if(booking.Period.Within(calendarDate.Date))
                    {
                        calendarDate.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id.Id });
                    }
                }

                rentalCalendar.Dates.Add(calendarDate);
            }

            return rentalCalendar;
        }
    }
}
