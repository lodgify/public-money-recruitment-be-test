using System.Linq;
using AutoFixture;
using FluentValidation;
using NUnit.Framework;
using VacationRental.BusinessLogic.Services.Validators;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Tests.Services.Validators
{
    [TestFixture]
    public class CreateBookingValidatorTests
    {
        private Fixture _autoFixture;
        private CreateBookingValidator _createBookingValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoFixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            _createBookingValidator = new CreateBookingValidator();
        }

        [Test]
        public void ValidateAndThrow_WhenNightsIs0_ThenThrowValidationException()
        {
            // Arrange
            var createBooking = _autoFixture
                .Build<CreateBooking>()
                .With(x => x.Nights, 0)
                .Create();

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => _createBookingValidator.ValidateAndThrow(createBooking));
            Assert.AreEqual("'Nights' must be greater than '0'.", ex.Errors.First().ToString());
        }

        [Test]
        public void ValidateAndThrow_WhenNightsIsNegative_ThenThrowValidationException()
        {
            // Arrange
            var createBooking = _autoFixture
                .Build<CreateBooking>()
                .With(x => x.Nights, -10)
                .Create();

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => _createBookingValidator.ValidateAndThrow(createBooking));
            Assert.AreEqual("'Nights' must be greater than '0'.", ex.Errors.First().ToString());
        }

        [Test]
        public void ValidateAndThrow_WhenModelIsValid_ThenPass()
        {
            // Arrange
            var createBooking = _autoFixture
                .Build<CreateBooking>()
                .With(x => x.Nights, 10)
                .Create();

            // Act
            _createBookingValidator.ValidateAndThrow(createBooking);

            // Assert
            Assert.Pass();
        }
    }
}
