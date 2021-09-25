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
        public void IsOverlapped__ParameterPeriodStarts_Before_Start_And_Ends_Before_Start__ReturnsFalse()
        {
            var period = new BookingPeriod(new DateTime(2001, 1, 1), 1);
            var anotherPeriod = new BookingPeriod(new DateTime(2000, 12, 1), 1);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriodStarts_After_End__ReturnsFalse()
        {
            var period = new BookingPeriod(new DateTime(2001, 1, 1), 1);
            var anotherPeriod = new BookingPeriod(new DateTime(2001, 2, 1), 1);

            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.False(isOverlapped);
        }
    }
}
