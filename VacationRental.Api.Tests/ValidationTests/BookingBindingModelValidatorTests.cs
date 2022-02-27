using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.ValidationTests
{
    public class BookingBindingModelValidatorTests
    {
        [Fact]
        public async Task Validate_WhenRequestIsNull_ThenThrowError()
        {
            // Arrange
            BookingBindingModelValidator stateUnderTest = new BookingBindingModelValidator();

            BookingBindingModel request = new BookingBindingModel();

            // Act
            TestValidationResult<BookingBindingModel> validationResult = await stateUnderTest.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
        }

        [Fact]
        public async Task Validate_WhenNightsIsNotGreaterThanZero_ThenThrowError()
        {
            // Arrange
            BookingBindingModelValidator stateUnderTest = new BookingBindingModelValidator();

            BookingBindingModel request = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.Nights, f => f.Random.Number(-10, 0));

            // Act
            TestValidationResult<BookingBindingModel> validationResult = await stateUnderTest.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ShouldHaveValidationErrorFor(r => r.Nights);
        }

        [Fact]
        public async Task Validate_WhenRentalIdIsNotGreaterThanZero_ThenThrowError()
        {
            // Arrange
            BookingBindingModelValidator stateUnderTest = new BookingBindingModelValidator();

            BookingBindingModel request = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, f => f.Random.Number(-10, 0));

            // Act
            TestValidationResult<BookingBindingModel> validationResult = await stateUnderTest.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ShouldHaveValidationErrorFor(r => r.RentalId);
        }

        [Fact]
        public async Task Validate_WhenStartIsMinDateValue_ThenThrowError()
        {
            // Arrange
            BookingBindingModelValidator stateUnderTest = new BookingBindingModelValidator();

            BookingBindingModel request = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.MinValue);

            // Act
            TestValidationResult<BookingBindingModel> validationResult = await stateUnderTest.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ShouldHaveValidationErrorFor(r => r.Start);
        }

        [Fact]
        public async Task Validate_WhenRequestIsAccurate_ThenReturnValid()
        {
            // Arrange
            BookingBindingModelValidator stateUnderTest = new BookingBindingModelValidator();

            BookingBindingModel request = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Today);

            // Act
            TestValidationResult<BookingBindingModel> validationResult = await stateUnderTest.TestValidateAsync(request);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
