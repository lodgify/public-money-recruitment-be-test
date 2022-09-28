using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<int> PreparationTimes { get; set; }
    }
}
