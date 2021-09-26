using System;
using VacationRental.Domain.Common;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Booking : Entity<BookingId>
    {
        //TODO: consider using a snapshot here. Too many parameters
        public Booking(BookingId id, RentalId rentalId, BookingPeriod period, int preparationTimeInDays, int unit) : base(id)
        {
            RentalId = rentalId;
            Period = period;
            Unit = unit;
            Preparation = new PreparationPeriod(period.GetEndOfPeriod(), preparationTimeInDays); // The preparation period starts when the booking is finished
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
        public PreparationPeriod Preparation { get; private set; }

        public void UpdatePreparationTime(int days) =>
            Preparation = new PreparationPeriod(Period.GetEndOfPeriod(), days);

        /// <summary>
        /// Checks whether the given and the booking period are overlapped
        /// </summary>
        /// <param name="periodToCompare">Period to check</param>
        /// <returns></returns>
        public bool IsOverlapped(BookingPeriod periodToCompare) => 
            Period.IsOverlapped(periodToCompare) || Preparation.IsOverlapped(periodToCompare);


        public bool IsOverlapped(Booking booking)
        {
            return Period.IsOverlapped(booking.Period)
                   || Period.IsOverlapped(booking.Preparation)
                   || Preparation.IsOverlapped(booking.Period)
                   || Preparation.IsOverlapped(booking.Preparation);
        }


        /// <summary>
        /// Returns the last day when the unit is blocked
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndOfBooking() => Preparation.GetEndOfPeriod();

        /// <summary>
        /// Returns the beginning of the booking 
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartOfBooking() => Period.Start;

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
