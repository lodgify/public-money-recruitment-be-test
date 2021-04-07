using System;

namespace VacationalRental.Domain.Models
{
    public class BookingModel
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
