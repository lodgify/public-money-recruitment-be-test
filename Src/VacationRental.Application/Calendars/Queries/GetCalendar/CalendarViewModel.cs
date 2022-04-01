using System.Collections.Generic;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
        
    }
}
