using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using VacationRental.Api.Models;

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

    }
}
