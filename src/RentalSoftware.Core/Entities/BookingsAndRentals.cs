using System;

namespace RentalSoftware.Core.Entities
{
    public class BookingsAndRentals
    {
        public int Id { get; set; }
        public Rental Rental { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfNights { get; set; }
    }
}
