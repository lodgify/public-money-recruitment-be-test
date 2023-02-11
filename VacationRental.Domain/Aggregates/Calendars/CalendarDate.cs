using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Domain.Aggregates.Calendars
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
        public List<PreparationTime> PreparationTimes { get; set; }

        public CalendarDate(DateTime date)
        {
            Date = date;
            Bookings = new List<CalendarBooking>();
            PreparationTimes = new List<PreparationTime>();
        }

        public void AggregateBookings(Booking booking)
        {
            var endBookingDate = booking.Start.AddDays(booking.Nights);

            if (booking.Start <= Date && endBookingDate > Date)
            {
                Bookings.Add(CalendarBooking.Create(booking.Id, booking.Units));
            }
        }

        public void DefinePreparationTimes(Booking booking, int preparationTimeInDays)
        {
            var endBookingDate = booking.Start.AddDays(booking.Nights);

            if (endBookingDate <= Date && endBookingDate.AddDays(preparationTimeInDays) > Date)
            {
                PreparationTimes.Add(new PreparationTime
                {
                    Unit = booking.Units
                });
            }                    
        }
    }
}
