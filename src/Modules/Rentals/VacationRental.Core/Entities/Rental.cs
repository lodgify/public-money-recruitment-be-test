using System;
using System.Collections.Generic;
using VacationRental.Core.Exceptions;

namespace VacationRental.Core.Entities
{
    internal class Rental
    {
        public int Id { get; private set; }
        public int Units { get; private set; }
        public int PreparationTimeInDays { get; private set; }
        public IList<Booking> Bookings { get; private set; } = new List<Booking>();

        private Rental()
        {
        }

        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public Booking CreateBooking(DateTime start, int nights, IEnumerable<Booking> bookings = null)
        {
            var unitsBooked = 0;

            foreach (var booking in bookings ?? Bookings)
            {
                if (booking.IsNotAvailable(start, nights, PreparationTimeInDays))
                {
                    unitsBooked++;
                }
            }

            if (unitsBooked >= Units)
                throw new BookingNotAvailableException();

            return new Booking(this, start, nights, unitsBooked + 1);
        }

        public void AddBooking(Booking newBooking) => Bookings.Add(newBooking);

        public void Update(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;

            var updatedBookings = new List<Booking>();

            foreach (var booking in Bookings)
            {
                try
                {
                    var updatedBooking = CreateBooking(booking.Start, booking.Nights, updatedBookings);
                    updatedBookings.Add(updatedBooking);
                }
                catch (BookingNotAvailableException)
                {
                    throw new RentalUpdateNotPossibleException(Id, Units, PreparationTimeInDays);
                }
            }
        }
    }
}
