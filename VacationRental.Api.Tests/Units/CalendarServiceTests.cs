using System;
using DeepEqual.Syntax;
using FakeItEasy;
using NUnit.Framework;
using VacationRental.Api.Models;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class CalendarServiceTests : BaseServiceTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldThrowApplicationExceptionIfNightsIsNotPositiveOnCreation(int nights)
        {
            var ex = Assert.Throws<ApplicationException>(() => CalendarService.GetCalendar(1, DateTime.Today, nights));

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Nights must be positive"));
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfRentalNotFoundOnCreation()
        {
            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(false);

            var ex = Assert.Throws<ApplicationException>(() => CalendarService.GetCalendar(1, DateTime.Today, 3));

            RentalNotFoundExceptionChecks(ex);
        }

        [Test]
        public void ShouldGenerateCalendarIfDataCorrectOnCreation()
        {
            var rental = new RentalViewModel()
            {
                Id = 1,
                PreparationTimeInDays = 2,
                Units = 3
            };

            var expectedResult = CalendarStubs.GenerateCalendar();

            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(true);
            A.CallTo(() => RentalRepository.Get(1)).Returns(rental);
            A.CallTo(() => BookingRepository.GetBookingsByRentalId(rental.Id)).Returns(BookingStubs.BookingWithCrossDays());

            var result = CalendarService.GetCalendar(1, new DateTime(2022, 09, 10), 9);

            Assert.AreEqual(expectedResult.Dates.Count, result.Dates.Count);
            expectedResult.ShouldDeepEqual(result);
        }
    }
}
