using System;
using System.Collections.Generic;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarRepository(IDictionary<int, RentalViewModel> rentals,
                IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public CalendarViewModel GetRentalCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var calendaryViewModel = CommonHelper.SetCalendarInstanceForRentalId(rentalId);

            for (var i = 0; i < nights; i++)
            {
                var date = start.SetCalendarDateInstanceFromStartDate(i);

                // TODO: add preparation time here
                foreach (var booking in _bookings.Values)
                {
                    if (booking.ValidateCalendarBookingsFromDates(date.Date, rentalId))
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                calendaryViewModel.Dates.Add(date);
            }

            return calendaryViewModel;
        }
    }

}
