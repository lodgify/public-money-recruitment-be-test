using System;
using System.Collections.Generic;

namespace VacationRental.Application.Calendars.Models
{
    public class CalendarDateViewModel
    {
        public CalendarDateViewModel(DateTime date)
        {
            Date = date;
            Bookings = new List<CalendarBookingViewModel>();
            PreparationTimes = new List<CalendarPreparationTimeViewModel>();
        }

        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<CalendarPreparationTimeViewModel> PreparationTimes { get; set; }
    }
}
