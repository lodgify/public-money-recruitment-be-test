using System;

namespace VacationRental.Services.Models.Booking
{
    public class CreateBookingRequest
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddDays(Nights);
        public int Nights { get; set; }
    }
}
