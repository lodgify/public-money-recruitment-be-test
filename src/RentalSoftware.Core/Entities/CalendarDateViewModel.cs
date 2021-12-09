using System;
using System.Collections.Generic;

namespace RentalSoftware.Core.Entities
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
        public List<PreparationTime> PreparationTimes { get; set; }
    }
}
