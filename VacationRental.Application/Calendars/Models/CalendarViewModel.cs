using System.Collections.Generic;

namespace VacationRental.Application.Calendars.Models
{
    public class CalendarViewModel
    {
        public CalendarViewModel(int rentalId)
        {
            RentalId = rentalId;
            Dates = new List<CalendarDateViewModel>();
        }

        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
