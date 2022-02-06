using System.Collections.Generic;
using VacationRental.Api.Models;

namespace Application.Models.Calendar.Responses
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarResponse> Dates { get; set; }
    }
}
