using System;
using VacationRental.Domain.Common;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Booking : Entity<BookingId>
    {
        public Booking(BookingId id, RentalId rentalId, BookingPeriod period, PreparationPeriod preparationPeriod, int unit) : base(id)
        {
            RentalId = rentalId;
            Period = period;
            Unit = unit;
            Preparation = preparationPeriod;
        }

        public RentalId RentalId { get;}
        public int Unit { get; }
        public BookingPeriod Period { get; }
        public PreparationPeriod Preparation { get; }

        public bool IsOverlapped(BookingPeriod periodToCompare) => 
            Period.IsOverlapped(periodToCompare) || Preparation.IsOverlapped(periodToCompare); //

        public bool WithinBookingPeriod(DateTime date) => Period.Within(date);
        public bool WithinPreparationPeriod(DateTime date) => Preparation.Within(date);
    }
}
