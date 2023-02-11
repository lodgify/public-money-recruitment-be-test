﻿using System;
using VacationRental.Domain.Primitives;

namespace VacationRental.Domain.Models.Bookings
{
    public sealed class Booking : BaseDomainModel
    {
        public int RentalId { get; private set; }
        public DateTime Start { get; private set; }
        public int Nights { get; private set; }
        public int Units { get; private set; }

        private Booking(int rentalId, DateTime start, int nights, int units) : base()
        {
            RentalId = rentalId;
            Start = start;
            Nights = nights;
            Units = units;
        }

        public static Booking Create(int rentalId, DateTime start, int nights, int units)
        {
            return new Booking(rentalId, start, nights, units);
        }

        public bool ShouldAddNewBookingToRental(DateTime start, int nights, int preparationTimeInDays)
        {
            var endBookingDate = Start.AddDays(Nights);
            return ((Start <= start.Date && endBookingDate > start.Date)
                || (Start < start.AddDays(nights) && endBookingDate >= start.AddDays(nights))
                    || (Start > start && endBookingDate < start.AddDays(nights))
                    || (endBookingDate <= Start && endBookingDate.AddDays(preparationTimeInDays) > Start));
        }
    }
}
