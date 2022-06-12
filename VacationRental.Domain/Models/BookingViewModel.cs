using System;

namespace VacationRental.Domain.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public DateTime End { get { return Start.AddDays(Nights); } }
    }
}
