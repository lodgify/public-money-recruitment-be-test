using System;
using System.Collections.Generic;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
    }
}
