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
            var bookedUnits = existingBookings
                .Where(booking => booking.IsOverlapped(newBookingPeriod))
                .Select(booking=>booking.Unit);

            var availableUnits = Enumerable.Range(1, Units)
                .Except(bookedUnits)
                .OrderBy(unit => unit);

            var firstAvailableUnit = availableUnits.FirstOrDefault();
            if (firstAvailableUnit == 0) // default value for Int32
            {
                throw new NoAvailableUnitException(Id);
            }

            return new Booking(BookingId.Empty, Id, newBookingPeriod,
                new PreparationPeriod(newBookingPeriod.GetEndOfPeriod(), // The preparation period starts when the booking is finished
                    PreparationTimeInDays), 
                firstAvailableUnit);
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
                throw new PreparationDaysLessThanOneException();
            }
        }
    }
}
