using System;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
    public class BookingPeriodTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void BookingPeriodConstructor_NightsIsLessThanOne_NightsLessThanOneException(int nights)
        {
            Assert.Throws<NightsLessThanOneException>(() => new BookingPeriod(new DateTime(2001, 1, 1), nights));
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_Before_Beginning_And_Ends_Before_Beginning__ReturnsFalse()
        {
            var period = new BookingPeriod(new DateTime(2001, 1, 1), 1);
            var anotherPeriod = new BookingPeriod(new DateTime(2000, 12, 1), 1);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_InTheEnd__ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddDays(nights), nights);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }


        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_After_Ending__ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 1;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddMonths(1), nights);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_Before_Beginning_And_Ends_After_Beginning__ReturnsTrue()
        {
            var start = new DateTime(2001, 2, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddDays(-2), nights);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_InTheBeginning_And_Ends_After_Beginning__ReturnsTrue()
        {
            var start = new DateTime(2001,1,1);
            var period = new BookingPeriod(start, 5);
            var anotherPeriod = new BookingPeriod(start, 3);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_Before_Ending_And_Ends_After_Ending__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddDays(nights - 1), nights);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_Before_Ending_And_Ends_InTheEnd__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddDays(nights - 1), 1);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_After_Beginning_And_Ends_Before_Ending__ReturnsTrue()
        {
            var start = new DateTime(2001,1,1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var anotherPeriod = new BookingPeriod(start.AddDays(1), nights-2);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }
    }
}
