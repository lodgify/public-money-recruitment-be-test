using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
    }
}
