using System;
using System.Collections.Generic;

namespace VacationRental.Services.Models.Calendar
{
    public class CalendarDateDto
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingDto> Bookings { get; set; }
        public List<CalendarPreparationTimeDto> PreparationTimes { get; set; }
    }
}
