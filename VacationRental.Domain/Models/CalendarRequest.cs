using System;

namespace VacationRental.Domain.Models
{
    public class CalendarRequest
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
