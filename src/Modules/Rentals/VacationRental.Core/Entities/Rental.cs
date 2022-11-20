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
        public IReadOnlyCollection<Booking> Bookings { get; private set; }

        private Rental()
        {
        }

        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public void SetRentalId(int id)
        {
            Id = id;
        }

        public Booking CreateBooking(DateTime start, int nights)
        {
            var unitsBooked = 0;

            foreach (var booking in Bookings)
            {
                if (booking.IsNotAvailable(start, nights))
                {
                    unitsBooked++;
                }
            }

            if (unitsBooked >= Units)
                throw new BookingNotAvailableException();

            return new Booking(this, start, nights, unitsBooked + 1);
        }
    }
}
