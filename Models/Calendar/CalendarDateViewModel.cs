using System;
using System.Collections.Generic;

namespace VacationRental.Models.Calendar
{ 
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<CalendarPreparationTime> PreparationTimes { get; set; }
    }
    
}