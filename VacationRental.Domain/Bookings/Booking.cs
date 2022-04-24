using VacationRental.Data;

namespace VacationRental.Domain.Bookings
{
    public class Booking : BaseEntity
    {
        public Booking()
        {
        }

        public Booking(int rentalId, int unit, DateTime start, int nights)
        {
            RentalId = rentalId;
            Unit = unit;
            Start = start;
            Nights = nights;
        }

        public int RentalId { get; set; }

        public int Unit { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
