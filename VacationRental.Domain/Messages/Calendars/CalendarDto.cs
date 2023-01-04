using System.Collections.Generic;

namespace VacationRental.Domain.Messages.Calendars
{
    public class CalendarDto
    {
        public int RentalId { get; set; }
        public List<CalendarDate> Dates { get; set; }
    }
}
