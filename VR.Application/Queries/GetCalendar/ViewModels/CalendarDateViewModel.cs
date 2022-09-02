using System;
using System.Collections.Generic;

namespace VR.Application.Queries.GetCalendar.ViewModels
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }

        public IList<CalendarBookingViewModel> Bookings { get; set; }

        public IList<PreparationTimeViewModel> PreparationTimes { get; set; }
    }
}
