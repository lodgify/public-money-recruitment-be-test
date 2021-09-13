using System;
using System.Collections.Generic;

namespace VacationRental.Application
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimesViewModel> PreparationTimes { get; set; }
    }
}
