using System;
using System.Collections.Generic;
using System.Linq;
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
            var rentalUnit = _rentals[rentalId].Units;

            for (var i = 0; i < nights; i++)
            {
                var date = start.SetCalendarDateInstanceFromStartDate(i);

                foreach (var booking in _bookings.Values)
                {
                    if (booking.ValidateCalendarBookingsFromDates(date.Date, rentalId))
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = rentalUnit });
                    }
                }
                calendaryViewModel.Dates.Add(date);
            }
            
            // Set preparation time for rentals
            var preparationTimeInDays = _rentals[rentalId].PreparationTimeInDays;
            if(preparationTimeInDays > 0)
            {
                for(var i = 0; i < preparationTimeInDays; i++)
                {
                    var addedDays = i + nights;
                    var date = start.SetCalendarDateInstanceFromStartDate(addedDays);
                    date.PreparationTimes.Add(new CalendarRentalUnitViewModel { Unit = rentalUnit });
                    calendaryViewModel.Dates.Add(date);
                }
            }

            return calendaryViewModel;
        }
    }

}
