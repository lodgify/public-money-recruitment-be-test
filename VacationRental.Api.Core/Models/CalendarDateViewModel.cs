using System;
using System.Collections.Generic;

namespace VacationRental.Api.Core.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<CalendarRentalUnitViewModel> PreparationTimes { get; set; }
    }
}
