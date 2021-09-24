using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Common;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Rental : Entity<RentalId>
    {
        public Rental(RentalId id, int units, int preparationTimeInDays) :  base(id)
        {
            CheckIfUnitsLessThanOne(units);
            CheckIfPreparationTimeLessThanOne(preparationTimeInDays);
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }
        public int Units { get; }
        public int PreparationTimeInDays { get; }

        public Booking Book(IReadOnlyCollection<Booking> existingBookings, BookingPeriod newBookingPeriod)
        {
            var amountOfBookedUnits = existingBookings.Count(booking => booking.Period.AddNights(PreparationTimeInDays).IsOverlapped(newBookingPeriod));
            if (amountOfBookedUnits == Units)
            {
                throw new NoAvailableUnitException(Id);
            }

            return new Booking(BookingId.Empty, Id, newBookingPeriod);
        }

        private static void CheckIfUnitsLessThanOne(int units)
        {
            if (units < 1)
            {
                throw new UnitsLessThanOneException();
            }
        }

        private static void CheckIfPreparationTimeLessThanOne(int preparationTimeInDays)
        {
            if (preparationTimeInDays < 1)
            {

            }
        }
    }
}
