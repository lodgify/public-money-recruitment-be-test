using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using Xunit.Sdk;

namespace VacationRental.Application.Tests.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidatorTests : IClassFixture<CreateBookingCommandValidatorTests>
    {
        private CreateBookingCommandValidator validator;

        public CreateBookingCommandValidatorTests()
        {
            validator = new();
        }

        [Theory]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateNightsInABooking(int nights, bool isValid, int errorNumbers)
        {
            //Arrange
            var request = new CreateBookingCommand(1, DateTime.Now, nights, 1);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumbers);            
        }        
    }
}
