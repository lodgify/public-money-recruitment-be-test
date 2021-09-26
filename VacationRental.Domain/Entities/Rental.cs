using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Common;
using VacationRental.Domain.Events.Rental;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Rental : Entity<RentalId>
    {
        private const int FirstUnitNumber = 1;

        public event RentalUpdatedHandler RentalUpdateEvent;

        public Rental(RentalId id, int units, int preparationTimeInDays) :  base(id)
        {
            Validate(units, preparationTimeInDays);
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public int Units { get; private set; }
        public int PreparationTimeInDays { get; private set; }

        private static void Validate(int units, int preparationTimeInDays)
        {
            CheckIfUnitsLessThanOne(units);
            CheckIfPreparationTimeLessThanOne(preparationTimeInDays);
        }

        public async Task Update(int units, int preparationTimeInDays)
        {
            Validate(units, preparationTimeInDays);
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
            if (RentalUpdateEvent != null)
            {
                await RentalUpdateEvent(new RentalUpdated(Id, units, preparationTimeInDays));
            }
        }

        public Booking Book(IReadOnlyCollection<Booking> existingBookings, BookingPeriod newBookingPeriod)
        {
            var bookedUnits = existingBookings
                .Where(booking => booking.IsOverlapped(newBookingPeriod))
                .Select(booking=>booking.Unit);

            var availableUnits = Enumerable.Range(FirstUnitNumber, Units)
                .Except(bookedUnits)
                .OrderBy(unit => unit);

            var firstAvailableUnit = availableUnits.FirstOrDefault();
            if (firstAvailableUnit == 0) // default value for Int32
            {
                throw new NoAvailableUnitException(Id);
            }

            return new Booking(BookingId.Empty, Id, newBookingPeriod,
                PreparationTimeInDays, firstAvailableUnit);
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
