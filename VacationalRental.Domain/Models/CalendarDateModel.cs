using System;
using System.Collections.Generic;

namespace VacationalRental.Domain.Models
{
    public class CalendarDateModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingModel> Bookings { get; set; }
        public List<PreparationTimesModel> PreparationTimes { get; set; }
    }
}
