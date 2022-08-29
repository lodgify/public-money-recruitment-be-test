using System.Collections.Generic;

namespace VacationRental.Services.Models.Calendar
{
    public class CalendarDto
    {
        public int RentalId { get; set; }
        public List<CalendarDateDto> Dates { get; set; }
    }
}
