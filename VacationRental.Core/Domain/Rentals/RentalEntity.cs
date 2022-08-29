using System.Collections.Generic;
using VacationRental.Core.Domain.Bookings;

namespace VacationRental.Core.Domain.Rentals
{
    public class RentalEntity : BaseEntity<int>
    {
        public RentalEntity()
        {
            Bookings = new List<BookingEntity>();
        }

        public int Units { get; set; }

        /// <summary>
        /// Preparation time in days 
        /// </summary>
        public int PreparationTime { get; set; }

        public ICollection<BookingEntity> Bookings { get; set; }
    }
}
