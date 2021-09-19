using System;

namespace VacationRental.Api.Models
{
    public class CalendarBindingModel
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; } 
        public int Nights { get; set; }
    }
}
