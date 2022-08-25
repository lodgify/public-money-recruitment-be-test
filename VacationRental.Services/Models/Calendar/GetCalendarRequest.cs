using System;

namespace VacationRental.Services.Models.Calendar
{
    public class GetCalendarRequest
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
