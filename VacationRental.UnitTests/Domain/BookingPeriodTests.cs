using System;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
    [Collection("Domain")]
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

            //Act
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

            //Act
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

            //Act
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

            //Act
            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void IsOverlapped__ParameterPeriod_Starts_InTheBeginning_And_Ends_After_Beginning__ReturnsTrue()
        {
            var start = new DateTime(2001,1,1);
            var period = new BookingPeriod(start, 5);
            var anotherPeriod = new BookingPeriod(start, 3);

            //Act
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

            //Act
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

            //Act
            var isOverlapped = period.IsOverlapped(anotherPeriod);

            Assert.True(isOverlapped);
        }

        [Fact]
        public void Within__Date_Is_Before_Beginning__ReturnsFalse()
        {
            var start = new DateTime(2001, 1,1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddMonths(-1);

            //Act
            var withinPeriod = period.Within(dateToCheck);

            Assert.False(withinPeriod);
        }

        [Fact]
        public void Within__Date_Is_InTheBeginning__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start;

            var withinPeriod = period.Within(dateToCheck);

            Assert.True(withinPeriod);
        }

        [Fact]
        public void Within__Date_Is_After_Beginning_Before_Ending__ReturnsTrue()
        {
            var start = new DateTime(2001, 1, 1);
            var nights = 5;
            var period = new BookingPeriod(start, nights);
            var dateToCheck = start.AddDays(1);

            //Act
            var withinPeriod = period.Within(dateToCheck);

            Assert.True(withinPeriod);
        }

        [Fact]
        public void Within__Date_Is_InTheEnd__ReturnsFalse()
        {
            var start = new DateTime(2001, 1,1);
            var night = 5;
            var period = new BookingPeriod(start, night);
            var dateToCheck = start.AddDays(night);

            //Act
            var withinPeriod = period.Within(dateToCheck);

            Assert.False(withinPeriod);
        }

        [Fact]
        public void Within__Date_Is_After_Ending_ReturnsFalse()
        {
            var start = new DateTime(2001, 1, 1);
            var night = 5;
            var period = new BookingPeriod(start, night);
            var dateToCheck = start.AddDays(night + 10);

            //Act
            var withinPeriod = period.Within(dateToCheck);

            Assert.False(withinPeriod);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void GetEndOfPeriod__NoParams__Returns_Start_Plus_Number_Of_Days(int days)
        {
            var start = new DateTime(2001,1,1);
            var period = new BookingPeriod(start, days);

            //Act
            var end = period.GetEndOfPeriod();

            var expectedValue = start.AddDays(days);
            Assert.Equal(expectedValue, end);
        }
    }
}
