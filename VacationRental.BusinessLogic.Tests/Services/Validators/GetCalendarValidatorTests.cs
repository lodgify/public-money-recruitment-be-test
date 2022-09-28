using System.Linq;
using AutoFixture;
using FluentValidation;
using NUnit.Framework;
using VacationRental.BusinessLogic.Services.Models;
using VacationRental.BusinessLogic.Services.Validators;

namespace VacationRental.BusinessLogic.Tests.Services.Validators
{
    [TestFixture]
    public class GetCalendarValidatorTests
    {
        private Fixture _autoFixture;
        private GetCalendarValidator _getCalendarValidator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoFixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            _getCalendarValidator = new GetCalendarValidator();
        }

        [Test]
        public void ValidateAndThrow_WhenNightsIs0_ThenThrowValidationException()
        {
            // Arrange
            var getCalendarServiceModel = _autoFixture
                .Build<GetCalendarServiceModel>()
                .With(x => x.Nights, 0)
                .Create();

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => _getCalendarValidator.ValidateAndThrow(getCalendarServiceModel));
            Assert.AreEqual("'Nights' must be greater than '0'.", ex.Errors.First().ToString());
        }

        [Test]
        public void ValidateAndThrow_WhenNightsIsNegative_ThenThrowValidationException()
        {
            // Arrange
            var getCalendarServiceModel = _autoFixture
                .Build<GetCalendarServiceModel>()
                .With(x => x.Nights, -10)
                .Create();

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => _getCalendarValidator.ValidateAndThrow(getCalendarServiceModel));
            Assert.AreEqual("'Nights' must be greater than '0'.", ex.Errors.First().ToString());
        }

        [Test]
        public void ValidateAndThrow_WhenModelIsValid_ThenPass()
        {
            // Arrange
            var getCalendarServiceModel = _autoFixture
                .Build<GetCalendarServiceModel>()
                .With(x => x.Nights, 10)
                .Create();

            // Act
            _getCalendarValidator.ValidateAndThrow(getCalendarServiceModel);

            // Assert
            Assert.Pass();
        }
    }
}
