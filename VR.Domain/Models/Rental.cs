using System.Collections.Generic;

namespace VR.Domain.Models
{
    public class Rental
    {
        public Rental()
        {
            Bookings = new List<Booking>();
        }

        public int Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
