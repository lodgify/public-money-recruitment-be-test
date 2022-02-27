using FluentAssertions;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.ValidationTests
{
    public class RentalBindingModelValidatorTests
    {
        [Fact]
        public async Task Validate_WhenRequestIsNull_ThenThrowError()
        {
            // Arrange
            RentalBindingModelValidator sut = new RentalBindingModelValidator();

            RentalBindingModel request = new RentalBindingModel();

            // Act
            TestValidationResult<RentalBindingModel> validationResult = await sut.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
        }

        [Fact]
        public async Task Validate_WhenUnitsIsNotGreaterThanZero_ThenThrowError()
        {
            // Arrange
            RentalBindingModelValidator sut = new RentalBindingModelValidator();

            RentalBindingModel request = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(-10, 0));

            // Act
            TestValidationResult<RentalBindingModel> validationResult = await sut.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ShouldHaveValidationErrorFor(r => r.Units);
        }

        [Fact]
        public async Task Validate_WhenPreparationTimeInDaysIsNotGreaterThanZero_ThenThrowError()
        {
            // Arrange
            RentalBindingModelValidator sut = new RentalBindingModelValidator();

            RentalBindingModel request = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(-10, 0));

            // Act
            TestValidationResult<RentalBindingModel> validationResult = await sut.TestValidateAsync(request);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ShouldHaveValidationErrorFor(r => r.PreparationTimeInDays);
        }

        [Fact]
        public async Task Validate_WhenRequestIsAccurate_ThenReturnValid()
        {
            // Arrange
            RentalBindingModelValidator sut = new RentalBindingModelValidator();

            RentalBindingModel request = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(1, 10));

            // Act
            TestValidationResult<RentalBindingModel> validationResult = await sut.TestValidateAsync(request);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
