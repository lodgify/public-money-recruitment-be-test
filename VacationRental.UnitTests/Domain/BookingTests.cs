using System;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
    [Collection("Domain")]
    public class BookingTests
    {
        [Fact]
        public void IsOverlapped__NewPeriod_Ends_Before_Beginning__ReturnsFalse()
        {
            var start = new DateTime(2001, 2, 1);
            var nights = 5;
            var preparationPeriod = 1;
            var unit = 1;

            var booking = new Booking(new BookingId(1), new RentalId(1), new BookingPeriod(start, nights), preparationPeriod,
                unit);

            var anotherStart = start.AddMonths(-1);
            var anotherPeriod = new BookingPeriod(anotherStart, nights);

            //Act
            var isOverlapped = booking.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__NewPeriod_Starts_After_Ending_Of_Preparation__ReturnsFalse()
        {
            var start = new DateTime(2001,1,1);
            var nights = 5;
            var preparationPeriod = 1;
            var unit = 1;

            var booking = new Booking(new BookingId(1), new RentalId(1), new BookingPeriod(start, nights),
                preparationPeriod, unit);

            var anotherStart = start.AddMonths(1);
            var anotherPeriod = new BookingPeriod(anotherStart, nights);

            //Act
            var isOverlapped = booking.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__NewPeriod_IsOverlapped_With_BookingPeriod__ReturnsTrue()
        {
            var start = new DateTime(2001, 1,1);
            var nights = 10;
            var preparationPeriod = 1;
            var unit = 1;

            var booking = new Booking(new BookingId(1), new RentalId(1), new BookingPeriod(start, nights),
                preparationPeriod, unit);

            var anotherStart = start.AddDays(1);
            var anotherPeriod = new BookingPeriod(anotherStart, nights);

            //Act
            var isOverlapped = booking.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }


        [Fact]
        public void IsOverlapped__NewPeriod_IsOverlapped_With_PreparationPeriod__ReturnsTrue()
        {
            var start = new DateTime(2001, 1,1);
            var nights = 10;
            var preparationPeriod = 5;
            var unit = 1;

            var booking = new Booking(new BookingId(1), new RentalId(1), new BookingPeriod(start, nights),
                preparationPeriod, unit);

            var anotherStart = start.AddDays(nights + 1); // overlapped with the preparation period
            var anotherPeriod = new BookingPeriod(anotherStart, nights);

            //Act
            var isOverlapped = booking.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__NewPeriod_IsOverlapped_With_BookingPeriod_And_PreparationPeriod__ReturnsTrue()
        {
            var start = new DateTime(2001,1,1);
            var nights = 10;
            var preparationPeriod = 5;
            var unit = 1;

            var booking = new Booking(new BookingId(1), new RentalId(1), new BookingPeriod(start, nights),
                preparationPeriod, unit);

            var anotherStart = start.AddDays(nights - 2); // starts before the end
            var anotherPeriod = new BookingPeriod(anotherStart, 5);

            //Act
            var isOverlapped = booking.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [Fact]
        public void WithinBookingPeriod__Date_Is_Before_Beginning__ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var preparationPeriod = 1;
            var unit = 1;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddMonths(-1);

            var booking = new Booking(new BookingId(1), new RentalId(1), period, preparationPeriod, unit);

            //Act
            var withinBookingPeriod = booking.WithinBookingPeriod(dateToCheck);

            Assert.False(withinBookingPeriod);
        }

        [Fact]
        public void WithinBookingPeriod__Date_Is_InTheBeginning__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var preparationPeriod = 1;
            var unit = 1;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start;

            var booking = new Booking(new BookingId(1), new RentalId(1), period, preparationPeriod, unit);

            //Act
            var withinBookingPeriod = booking.WithinBookingPeriod(dateToCheck);

            Assert.True(withinBookingPeriod);
        }

        [Fact]
        public void WithinBookingPeriod__Date_Is_After_Beginning_Before_Ending__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var unit = 1;
            var preparationPeriod = 1;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddDays(1);

            var booking = new Booking(new BookingId(1), new RentalId(1), period,
                preparationPeriod, unit);

            //Act
            var withinPeriod = booking.WithinBookingPeriod(dateToCheck);

            Assert.True(withinPeriod);
        }

        [Fact]
        public void WithinBookingPeriod__Date_Is_InTheEnd__ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var unit = 1;
            var preparationPeriod = 1;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddDays(nights);

            var booking = new Booking(new BookingId(1), new RentalId(1), period, preparationPeriod, unit);

            //Act
            var withinPeriod = booking.WithinBookingPeriod(dateToCheck);

            Assert.False(withinPeriod);
        }

        [Fact]
        public void WithinBookingPeriod__Date_Is_After_Ending_ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var unit = 1;
            var preparationPeriod = 1;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddDays(nights + 10);

            var booking = new Booking(new BookingId(1),  new RentalId(1), period, preparationPeriod, unit);

            //Act
            var withinPeriod = booking.WithinBookingPeriod(dateToCheck);

            Assert.False(withinPeriod);
        }
    }
}
