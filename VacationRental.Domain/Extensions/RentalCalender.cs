using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Extensions
{
    public static class RentalCalender
    {
        public static CalendarViewModel GenerateCalender(this RentalViewModel rental, IEnumerable<BookingViewModel> bookings, int rentalId, DateTime start, int nights)
        {
            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                };

                foreach (var booking in bookings)
                {
                    int unitAvailable = 1 + date.Bookings.Count + date.PreparationTimes.Count;

                    if (booking.Start <= date.Date && booking.End > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = unitAvailable });
                    }

                    if (booking.End <= date.Date && booking.End.AddDays(rental.PreparationTimeInDays) > date.Date)
                    {
                        int? unit = GetUnitFromBookings(result.Dates, booking.Id);
                        if (!unit.HasValue) unit = unitAvailable;
                        date.PreparationTimes.Add(new CalendarPreparationTimeViewModel { Unit = unit.Value });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }

        private static int? GetUnitFromBookings(List<CalendarDateViewModel> dateCalendar, int bookingId)
        {
            foreach (var item in dateCalendar)
            {
                var booking = item.Bookings.Where(e => e.Id == bookingId).FirstOrDefault();
                if (booking != null) return booking.Unit;
            }

            return null;
        }
    }
}
