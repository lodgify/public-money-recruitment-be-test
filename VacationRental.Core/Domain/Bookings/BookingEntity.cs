using System;
using VacationRental.Core.Domain.Rentals;

namespace VacationRental.Core.Domain.Bookings
{
    public class BookingEntity : BaseEntity<int>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddDays(Nights);
        public int Nights { get; set; }
        public int Unit { get; set; }

        #region Navigation properties

        public RentalEntity Rental { get; set; }

        #endregion

        public override string ToString()
        {
            return $"Id: {Id}, rentalId: {RentalId} between [{Start:yyyy-MM-dd} and {End:yyyy-MM-dd}] for {Nights} night(s)";
        }
    }
}
