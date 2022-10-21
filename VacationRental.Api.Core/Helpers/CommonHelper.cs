using System;
using System.Collections.Generic;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Helpers
{
    internal static class CommonHelper
    {
        public static ResourceIdViewModel CreateResourceIdForRentals(this IDictionary<int, RentalViewModel> rentals)
            => new ResourceIdViewModel { Id = rentals.Keys.Count + 1 };

        public static ResourceIdViewModel CreateResourceIdForBookings(this IDictionary<int, BookingViewModel> bookings)
            => new ResourceIdViewModel { Id = bookings.Keys.Count + 1 };

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

        public static bool ValidateBookingDates(BookingViewModel booking, BookingBindingModel bookingModel)
        {
            return booking.RentalId == bookingModel.RentalId
                    && (booking.Start <= bookingModel.Start.Date && booking.Start.AddDays(booking.Nights) > bookingModel.Start.Date)
                    || (booking.Start < bookingModel.Start.AddDays(bookingModel.Nights) && booking.Start.AddDays(booking.Nights) >= bookingModel.Start.AddDays(bookingModel.Nights))
                        || (booking.Start > bookingModel.Start && booking.Start.AddDays(booking.Nights) < bookingModel.Start.AddDays(bookingModel.Nights));
        }

        public static bool ValidateCalendarBookingsFromDates(this BookingViewModel bookingViewModel, DateTime date, int rentalId)
        {
            return bookingViewModel.RentalId == rentalId
                        && bookingViewModel.Start <= date.Date && bookingViewModel.Start.AddDays(bookingViewModel.Nights) > date.Date;
        }
    }
}
