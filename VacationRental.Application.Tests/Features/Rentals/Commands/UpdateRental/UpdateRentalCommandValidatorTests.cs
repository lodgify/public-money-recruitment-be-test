using FluentValidation;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Application.Features.Rentals.Commands.UpdateRental;

namespace VacationRental.Application.Tests.Features.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandValidatorTests : IClassFixture<UpdateRentalCommandValidatorTests>
    {
        private UpdateRentalCommandValidator _validator;

        public UpdateRentalCommandValidatorTests()
        {
            _validator = new();
        }


        [Theory]
        [InlineData(0, false, 1)]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateRentalId(int rentalId, bool isValid, int errorNumber)
        {
            var request = new UpdateRentalCommand(rentalId, 1, 1);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }

        [Theory]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateUnits(int units, bool isValid, int errorNumber)
        {
            var request = new UpdateRentalCommand(1, units, 1);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }

        [Theory]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidatePreparationTimeInDays(int preparationTimeInDays, bool isValid, int errorNumber)
        {
            var request = new UpdateRentalCommand(1, 1, preparationTimeInDays);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }
    }
}
