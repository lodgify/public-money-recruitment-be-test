using System;

namespace VacationRental.Core.Entities
{
    internal class Booking
    {
        public int Id { get; private set; }

        public int RentalId { get; private set; }

        public DateTime Start { get; private set; }

        public int Nights { get; private set; }

        public Booking(int rentalId, DateTime start, int nights)
        {
            RentalId = rentalId;
            Start = start;
            Nights = nights;
        }

        public void SetBookingId(int bookingId)
        {
            Id = bookingId;
        }

        public bool IsNotAvailable(DateTime start, int nights)
        {
            return (Start <= start && Start.AddDays(Nights) > start.Date)
                   || (Start < start.AddDays(nights) && Start.AddDays(Nights) >= start.AddDays(nights))
                   || (Start > start && Start.AddDays(Nights) < start.AddDays(nights));
        }

    }
}
