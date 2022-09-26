using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using VacationRental.BusinessLogic.Services;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessLogic.Services.Models;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class CalendarsServiceTests
    {
        private Mock<IBookingsService> _bookingsServiceMock;
        private Mock<IRentalsService> _rentalsServiceMock;
        private Mock<IValidator<GetCalendarServiceModel>> _getCalendarValidatorMock;
        private CalendarsService _calendarsService;

        private Fixture _autoFixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoFixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            _bookingsServiceMock = new Mock<IBookingsService>(MockBehavior.Strict);
            _getCalendarValidatorMock = new Mock<IValidator<GetCalendarServiceModel>>(MockBehavior.Strict);
            _rentalsServiceMock = new Mock<IRentalsService>(MockBehavior.Strict);

            _calendarsService = new CalendarsService(_bookingsServiceMock.Object, _rentalsServiceMock.Object,
                _getCalendarValidatorMock.Object);
        }

        [Test]
        public void GetCalendar_WhenRentalNotExists_ThenThrowArgumentException()
        {
            // Arrange
            var rentalId = _autoFixture.Create<int>();
            var start = _autoFixture.Create<DateTime>();
            var nights = _autoFixture.Create<int>();
            _getCalendarValidatorMock.Setup(p => p.Validate(It.Is<ValidationContext<GetCalendarServiceModel>>(context => context.ThrowOnFailures)))
                .Returns(new ValidationResult { Errors = new List<ValidationFailure>() });
            _rentalsServiceMock.Setup(x => x.GetRental(rentalId)).Returns((Rental)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _calendarsService.GetCalendar(rentalId, start, nights));
            Assert.AreEqual($"Rental with id {rentalId} not found.", ex.Message);
            _bookingsServiceMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
            _getCalendarValidatorMock.VerifyAll();
        }

        [Test]
        // A small unit test. In real application - add more unit tests for each booking condition 
        public void GetCalendar_WhenRentalExists_ThenReturnCalendar()
        {
            // Arrange
            var rentalId = _autoFixture.Create<int>();
            var start = _autoFixture.Create<DateTime>();
            var nights = _autoFixture.Create<int>();
            var rental = _autoFixture.Create<Rental>();
            var bookings = _autoFixture
                .Build<Booking>()
                .With(x => x.RentalId, rentalId)
                .With(x => x.Start, start.Add(TimeSpan.FromDays(-2)))
                .With(x => x.Nights, 5)
                .CreateMany(2)
                .ToList();
            _getCalendarValidatorMock.Setup(p => p.Validate(It.Is<ValidationContext<GetCalendarServiceModel>>(context => context.ThrowOnFailures)))
                .Returns(new ValidationResult { Errors = new List<ValidationFailure>() });
            _rentalsServiceMock.Setup(x => x.GetRental(rentalId)).Returns(rental);
            _bookingsServiceMock.Setup(x => x.GetBookings()).Returns(bookings);

            // Act
            var result = _calendarsService.GetCalendar(rentalId, start, nights);

            // Assert
            Assert.AreEqual(rentalId, result.RentalId);
            _bookingsServiceMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
            _getCalendarValidatorMock.VerifyAll();
        }
    }
}
