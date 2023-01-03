using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
        public List<PreparationTime> PreparationTimes { get; set; }
    }
}
