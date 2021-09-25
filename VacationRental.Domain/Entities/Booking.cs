using System;
using VacationRental.Domain.Common;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Booking : Entity<BookingId>
    {
        //TODO: consider using a snapshot here. Too many parameters
        public Booking(BookingId id, RentalId rentalId, BookingPeriod period, PreparationPeriod preparationPeriod, int unit) : base(id)
        {
            RentalId = rentalId;
            Period = period;
            Unit = unit;
            Preparation = preparationPeriod;
        }

        public RentalId RentalId { get;}
        public int Unit { get; }

        /// <summary>
        /// Booking period
        /// </summary>
        public BookingPeriod Period { get; }

        /// <summary>
        /// Preparation period
        /// </summary>
        public PreparationPeriod Preparation { get; }

        /// <summary>
        /// Checks whether the given and the booking period are overlapped
        /// </summary>
        /// <param name="periodToCompare">Period to check</param>
        /// <returns></returns>
        public bool IsOverlapped(BookingPeriod periodToCompare) => 
            Period.IsOverlapped(periodToCompare) || Preparation.IsOverlapped(periodToCompare);

        /// <summary>
        /// Checks whether the given date is within the booking period
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns></returns>
        public bool WithinBookingPeriod(DateTime date) => Period.Within(date);

        /// <summary>
        ///  Checks whether the given date is within the preparation period
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool WithinPreparationPeriod(DateTime date) => Preparation.Within(date);
    }
}
