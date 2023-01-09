using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Application.Features.Rentals.Queries.GetRental;

namespace VacationRental.Application.Tests.Features.Rentals.Queries
{
    public class GetRentalQueryValidatorTests : IClassFixture<GetRentalQueryValidatorTests>
    {
        private GetRentalQueryValidator _validator;

        public GetRentalQueryValidatorTests()
        {
            _validator = new GetRentalQueryValidator();
        }

        [Theory]
        [InlineData(null, false, 1)]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateRentalId(int rentalId, bool isValid, int errorNumber)
        {
            //Arrange
            var request = new GetRentalQuery(rentalId);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }

    }
}
