using System;
using System.Collections.Generic;

namespace VacationRental.Application.Queries.Calendar.ViewModel
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimesViewModel> PreparationTimes { get; set; }
    }
}
