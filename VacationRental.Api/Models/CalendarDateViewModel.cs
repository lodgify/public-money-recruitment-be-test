using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public HashSet<CalendarBookingViewModel> Bookings { get; set; } = new();
        public HashSet<CalendarPreparationTimeViewModel> PreparationTimes { get; set; } = new();
        
        public override int GetHashCode() => Date.GetHashCode();
    }
}
