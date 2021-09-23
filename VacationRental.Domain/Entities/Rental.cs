using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Rental
    {
        public Rental(RentalId id, int units, int preparationTimeInDays)
        {
            Id = id;
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public RentalId Id { get; }
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
    }
}
