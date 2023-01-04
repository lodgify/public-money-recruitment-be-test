using System;

namespace VacationRental.Domain.Messages.Bookings
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
