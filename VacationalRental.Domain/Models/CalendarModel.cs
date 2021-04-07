using System.Collections.Generic;

namespace VacationalRental.Domain.Models
{
    public class CalendarModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateModel> Dates { get; set; }
    }
}
