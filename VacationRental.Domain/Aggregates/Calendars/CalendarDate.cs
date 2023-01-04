using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Domain.Aggregates.Calendars
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
        public List<PreparationTime> PreparationTimes { get; set; }
    }
}
