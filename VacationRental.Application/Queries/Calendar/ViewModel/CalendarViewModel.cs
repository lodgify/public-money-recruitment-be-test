using System.Collections.Generic;

namespace VacationRental.Application.Queries.Calendar.ViewModel
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
