using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => this.Start.AddDays(this.Nights);
        public double PreparationTimeInDays { get; set; }
        public DateTime  EndPreperations => this.End.AddDays(this.PreparationTimeInDays);
        public int Nights { get; set; }
        public int Units { get; set; }
        
    }
}
