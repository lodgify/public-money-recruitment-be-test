using System;
using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
    public class RentalTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void RentalConstructor_AllArgumentsAreValid_But_UnitsIsLessThanOne_UnitsLessThanOneExceptionThrown(
            int units)
        {
            Assert.Throws<UnitsLessThanOneException>(() => new Rental(new RentalId(1), units, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void
            RentalConstructor_AllArgumentsAreValid_But_UnitsIsLessThanOne_PreparationDaysLessThanOneExceptionThrown(
                int preparationInDays)
        {
            Assert.Throws<PreparationDaysLessThanOneException>(() => new Rental(new RentalId(1), 1, preparationInDays));
        }

        [Fact]
        public void Book_RentalWithNoBooking_BookingCreated()
        {
            var units = 1;
            var preparationTime = 1;
            var rental = new Rental(new RentalId(1), units, preparationTime);

            var start = new DateTime(2001, 1, 1);
            var nights = 10;
            var startOfPreparation = start.AddDays(nights);

            //Act
            var booking = rental.Book(new List<Booking>(), new BookingPeriod(start, nights));

            Assert.NotNull(booking);
            Assert.Equal(start, booking.Period.Start);
            Assert.Equal(nights, booking.Period.Nights);
            Assert.Equal(preparationTime, booking.Preparation.Days);
            Assert.Equal(startOfPreparation, booking.Preparation.Start);
            Assert.Equal(1, booking.Unit); //first available
        }

        [Fact]
        public void Book_RentalWithOneAvailableUnit_BookingCreated()
        {
            var units = 2;
            var preparationTime = 1;
            var start = new DateTime(2001, 1, 1);
            var nights = 10;
            var startOfPreparation = start.AddDays(nights);
            var numberOfUnits = 1;

            var rental = new Rental(new RentalId(1), units, preparationTime);

            var bookings = new List<Booking>
            {
                new Booking(new BookingId(1), rental.Id, new BookingPeriod(start, nights),
                    new PreparationPeriod(startOfPreparation, preparationTime), numberOfUnits)
            };

            //Act
            var booking = rental.Book(bookings, new BookingPeriod(start, nights));

            Assert.NotNull(booking);
            Assert.Equal(start, booking.Period.Start);
            Assert.Equal(nights, booking.Period.Nights);
            Assert.Equal(preparationTime, booking.Preparation.Days);
            Assert.Equal(startOfPreparation, booking.Preparation.Start);
            Assert.Equal(2, booking.Unit);
        }

        [Fact]
        public void Book_RentalWithNoAvailableUnits_NoAvailableUnitExceptionThrown()
        {
            var units = 1;
            var preparationTime = 1;
            var start = new DateTime(2001, 1, 1);
            var nights = 10;
            var startOfPreparation = start.AddDays(nights);
            var numberOfUnits = 1;

            var rental = new Rental(new RentalId(1), units, preparationTime);

            var bookings = new List<Booking>
            {
                new Booking(new BookingId(1), rental.Id, new BookingPeriod(start, nights),
                    new PreparationPeriod(startOfPreparation, preparationTime), numberOfUnits)
            };
            
            //Act
            Assert.Throws<NoAvailableUnitException>(() => rental.Book(bookings, new BookingPeriod(start, nights)));
        }


        [Fact]
        public void Book_RentalWithNoOverlappedPeriod_BookingCreated()
        {
            var units = 1;
            var preparationTime = 1;
            var start = new DateTime(2001, 1, 1);
            var nights = 10;
            var startOfPreparation = start.AddDays(nights);

            var rental = new Rental(new RentalId(1), units, preparationTime);

            var existingBookingsDate = start.AddMonths(1);
            var bookings = new List<Booking>
            {
                //The booking is a month later than that one we're booking below
                new Booking(new BookingId(1), rental.Id, new BookingPeriod(existingBookingsDate, nights),
                    new PreparationPeriod(existingBookingsDate.AddDays(nights), preparationTime), 1)
            };

            //Act
            var booking = rental.Book(bookings, new BookingPeriod(start, nights));

            Assert.NotNull(booking);
            Assert.Equal(start, booking.Period.Start);
            Assert.Equal(nights, booking.Period.Nights);
            Assert.Equal(preparationTime, booking.Preparation.Days);
            Assert.Equal(startOfPreparation, booking.Preparation.Start);
            Assert.Equal(1, booking.Unit);
        }
    }
}
