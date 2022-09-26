using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using VacationRental.BusinessLogic.Mapping;
using VacationRental.BusinessLogic.Services;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class BookingsServiceTests
    {
        private Mock<IBookingsRepository> _bookingsRepositoryMock;
        private Mock<IValidator<CreateBooking>> _createBookingValidatorMock;
        private Mock<IRentalsService> _rentalsServiceMock;
        private BookingsService _bookingsService;

        private Fixture _autoFixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoFixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            var mappingProfile = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookingsServiceMappingProfile>();
            });
            var mapper = mappingProfile.CreateMapper();

            _bookingsRepositoryMock = new Mock<IBookingsRepository>(MockBehavior.Strict);
            _createBookingValidatorMock = new Mock<IValidator<CreateBooking>>(MockBehavior.Strict);
            _rentalsServiceMock = new Mock<IRentalsService>(MockBehavior.Strict);

            _bookingsService = new BookingsService(_bookingsRepositoryMock.Object, mapper,
                _createBookingValidatorMock.Object, _rentalsServiceMock.Object);
        }

        [Test]
        public void GetBooking_WhenBookingNotExists_ThenThrowArgumentException()
        {
            // Arrange
            var bookingId = _autoFixture.Create<int>();
            _bookingsRepositoryMock.Setup(x => x.GetBookingEntity(bookingId)).Returns((BookingEntity)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _bookingsService.GetBooking(bookingId));
            Assert.AreEqual($"Booking with id {bookingId} does not exist.", ex.Message);
            _bookingsRepositoryMock.VerifyAll();
            _createBookingValidatorMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
        }

        [Test]
        public void GetBooking_WhenBookingExist_ThenReturnMappedBooking()
        {
            // Arrange
            var bookingId = _autoFixture.Create<int>();
            var bookingEntity = _autoFixture.Create<BookingEntity>();
            _bookingsRepositoryMock.Setup(x => x.GetBookingEntity(bookingId)).Returns(bookingEntity);

            // Act
            var result = _bookingsService.GetBooking(bookingId);

            // Assert
            Assert.AreEqual(bookingEntity.Id, result.Id);
            Assert.AreEqual(bookingEntity.Start, result.Start);
            Assert.AreEqual(bookingEntity.Nights, result.Nights);
            Assert.AreEqual(bookingEntity.RentalId, result.RentalId);
            _bookingsRepositoryMock.VerifyAll();
            _createBookingValidatorMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
        }

        [Test]
        public void CreateBooking_WhenRentalNotExists_ThenThrowArgumentException()
        {
            // Arrange
            var createBooking = _autoFixture.Create<CreateBooking>();
            _createBookingValidatorMock.Setup(p => p.Validate(It.Is<ValidationContext<CreateBooking>>(context => context.ThrowOnFailures)))
                .Returns(new ValidationResult{Errors = new List<ValidationFailure>()});
            _rentalsServiceMock.Setup(x => x.GetRental(createBooking.RentalId)).Returns((Rental)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _bookingsService.CreateBooking(createBooking));
            Assert.AreEqual($"Rental with id {createBooking.RentalId} not found.", ex.Message);
            _bookingsRepositoryMock.VerifyAll();
            _createBookingValidatorMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
        }

        [Test]
        // We should have at least 2 more unit tests for each "Not available." condition  in real code
        public void CreateBooking_WhenUnitIsNotAvailable_ThenThrowApplicationException()
        {
            // Arrange
            var createBooking = _autoFixture.Create<CreateBooking>();
            var rental = _autoFixture
                .Build<Rental>()
                .With(x => x.Units, 1)
                .Create();
            var bookingEntities = _autoFixture
                .Build<BookingEntity>()
                .With(x => x.RentalId, createBooking.RentalId)
                .With(x => x.Start, createBooking.Start.Date.AddDays(-10))
                .With(x => x.Nights, 11)
                .CreateMany(2)
                .ToList();
            _bookingsRepositoryMock.Setup(x => x.GetBookingEntities()).Returns(bookingEntities);
            _createBookingValidatorMock.Setup(p => p.Validate(It.Is<ValidationContext<CreateBooking>>(context => context.ThrowOnFailures)))
                .Returns(new ValidationResult{Errors = new List<ValidationFailure>()});
            _rentalsServiceMock.Setup(x => x.GetRental(createBooking.RentalId)).Returns(rental);

            // Act & Assert
            var ex = Assert.Throws<ApplicationException>(() => _bookingsService.CreateBooking(createBooking));
            Assert.AreEqual("Not available.", ex.Message);
            _bookingsRepositoryMock.VerifyAll();
            _createBookingValidatorMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
        }

        [Test]
        // We should have at least 2 more unit tests for each "available" condition in real code
        public void CreateBooking_WhenUnitIsAvailable_ThenCreateBooking()
        {
            // Arrange
            var createBooking = _autoFixture.Create<CreateBooking>();
            var rental = _autoFixture
                .Build<Rental>()
                .With(x => x.Units, 1)
                .Create();
            var bookingEntities = _autoFixture
                .Build<BookingEntity>()
                .With(x => x.RentalId, createBooking.RentalId)
                .With(x => x.Start, createBooking.Start.Date.AddDays(-10))
                .With(x => x.Nights, 10)
                .CreateMany(2)
                .ToList();
            var bookingEntityId = _autoFixture.Create<int>();
            _bookingsRepositoryMock.Setup(x => x.GetBookingEntities()).Returns(bookingEntities);
            _createBookingValidatorMock.Setup(p => p.Validate(It.Is<ValidationContext<CreateBooking>>(context => context.ThrowOnFailures)))
                .Returns(new ValidationResult{Errors = new List<ValidationFailure>()});
            _rentalsServiceMock.Setup(x => x.GetRental(createBooking.RentalId)).Returns(rental);
            _bookingsRepositoryMock.Setup(x => x.CreateBookingEntity(It.IsAny<BookingEntity>()))
                .Returns(bookingEntityId);

            // Act
            var result = _bookingsService.CreateBooking(createBooking);

            // Assert
            Assert.AreEqual(bookingEntityId, result);
            _bookingsRepositoryMock.VerifyAll();
            _createBookingValidatorMock.VerifyAll();
            _rentalsServiceMock.VerifyAll();
        }
    }
}
