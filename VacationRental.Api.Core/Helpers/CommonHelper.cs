using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Core.Helpers
{
    internal static class CommonHelper
    {
        public static CalendarViewModel SetCalendarInstanceForRentalId(int rentalId)
        {
            return new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
        }

        public static CalendarDateViewModel SetCalendarDateInstanceFromStartDate(this DateTime startDate, int days)
        {
            return new CalendarDateViewModel
            {
                Date = startDate.Date.AddDays(days),
                Bookings = new List<CalendarBookingViewModel>(),
                PreparationTimes = new List<CalendarRentalUnitViewModel>()
            };
        }

        public static bool CheckOccupancyAvailability(BookingViewModel booking, BookingBindingModel newBooking, int preparationDate)
        {
            var occupiedDays = newBooking.Nights + preparationDate;
            var currentBooking = booking.Nights + preparationDate -1;
            return  booking.Start <= newBooking.Start.Date && booking.Start.AddDays(currentBooking) > newBooking.Start.Date
                    || (booking.Start < newBooking.Start.AddDays(occupiedDays) && booking.Start.AddDays(currentBooking) >= newBooking.Start.AddDays(occupiedDays))
                    || (booking.Start > newBooking.Start && booking.Start.AddDays(currentBooking) < newBooking.Start.AddDays(occupiedDays));
        }

        public static bool ValidateCalendarBookingsFromDates(this BookingViewModel bookingViewModel, DateTime date, int rentalId)
        {
            return bookingViewModel.RentalId == rentalId
                        && bookingViewModel.Start <= date.Date && bookingViewModel.Start.AddDays(bookingViewModel.Nights) > date.Date;
        }

        public static BookingViewModel ToBookingDto(this BookingBindingModel bindingModel)
            => new BookingViewModel { Id = 0, Nights = bindingModel.Nights, RentalId = bindingModel.RentalId, Start = bindingModel.Start };

        public static bool CheckConflictBookings(IEnumerable<BookingViewModel> bookings, RentalViewModel rental, RentalBindingModel rentalBindingModel)
        {
            foreach (var booking in bookings)
            {
                var newTotalBookedDays = booking.Start.AddDays(booking.Nights + rentalBindingModel.PreparationTimeInDays);
                var currentTotalBookedDays = booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays);
                if (newTotalBookedDays > currentTotalBookedDays)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
