using System.Collections.Generic;

namespace VacationRental.Models.Calendar
{ 
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
