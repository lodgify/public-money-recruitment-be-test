using System;

namespace VacationRental.Services.Models.Booking
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
