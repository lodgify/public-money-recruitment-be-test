using System;
using System.Collections.Generic;
using VacationRental.Booking.Domain.Interfaces;

namespace VacationRental.Booking.Domain
{

    public class CalendarBooking : ICalendarBooking
    {
        public int Id { get; set; }

        public int Unit { get; set; }

    }

    public class CalendarUnit : IBookingUnit
    {
        public int Unit { get; set; }
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public IList<CalendarBooking> Bookings { get; set; }
        public IList<CalendarUnit> PreparationTimes { get; set; }

        public CalendarDay(DateTime date, IList<CalendarBooking> bookings, IList<CalendarUnit> preparationBookings)
        {
            this.Update(date, bookings, preparationBookings);
        }

        private void Update(DateTime date, IList<CalendarBooking> bookings, IList<CalendarUnit> preparationBookings)
        {
            this.Date = date;
            this.Bookings = bookings;
            this.PreparationTimes = preparationBookings;
        }
    }

}
