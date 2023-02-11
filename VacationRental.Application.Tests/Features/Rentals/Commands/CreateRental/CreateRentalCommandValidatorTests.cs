using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;

namespace VacationRental.Application.Tests.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandValidatorTests : IClassFixture<CreateRentalCommandValidatorTests>
    {
        private CreateRentalCommandValidator _validator;

        public CreateRentalCommandValidatorTests()
        {
            _validator = new();
        }

        [Theory]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateUnits(int units, bool isValid, int errorNumber)
        {
            var request = new CreateRentalCommand(units, 1);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }

        [Theory]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidatePreparationInDays(int preparationInDays, bool isValid, int errorNumber)
        {
            var request = new CreateRentalCommand(1, preparationInDays);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }
    }
}
