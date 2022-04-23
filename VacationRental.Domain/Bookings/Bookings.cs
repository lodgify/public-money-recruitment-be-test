using VacationRental.Data;

namespace VacationRental.Domain.Bookings
{
    public class Bookings : BaseEntity
    {
        public Bookings(int rentalId, int unit, DateTime start, int nights)
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
