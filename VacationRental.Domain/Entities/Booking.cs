using VacationRental.Domain.Common;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Booking : Entity<BookingId>
    {
        public Booking(BookingId id, RentalId rentalId, BookingPeriod period)
        {
            
        }

        public BookingId Id { get; }
        public RentalId RentalId { get;}
        public BookingPeriod Period { get; }
    }
}
