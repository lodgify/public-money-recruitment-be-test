using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace Application.Models.Calendar.Responses
{
    public class CalendarResponse
    {
        public DateTime Date { get; set; }
        
        public List<CalendarBookingViewModel> Bookings { get; set; }
        
        public List<PreparationTime> PreparationTimes { get; set; }
        
    }
}
