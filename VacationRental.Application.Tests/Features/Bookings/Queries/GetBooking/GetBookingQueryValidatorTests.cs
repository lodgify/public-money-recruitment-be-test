using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using VacationRental.Application.Features.Bookings.Queries.GetBooking;

namespace VacationRental.Application.Tests.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryValidatorTests : IClassFixture<GetBookingQueryValidatorTests>
    {
        private GetBookingQueryValidator _validator;

        public GetBookingQueryValidatorTests()
        {
            _validator = new();
        }

        [Theory]
        [InlineData(null, false, 1)]
        [InlineData(-2, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateBookingId(int bookingId, bool isValid, int errorNumber)
        {
            var request = new GetBookingQuery(bookingId);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }
    }
}
