using System;
using VacationRental.Core.Domain.Rentals;

namespace VacationRental.Core.Domain.Bookings
{
    public class BookingEntity : BaseEntity<int>
    {
        public RentalEntity Rental { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
