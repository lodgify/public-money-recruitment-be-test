using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;

namespace VacationRental.Application.Tests.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryValidatorTests : IClassFixture<GetRentalCalendarQueryValidatorTests>
    {
        private GetRentalCalendarQueryValidator _validator;

        public GetRentalCalendarQueryValidatorTests()
        {
            _validator = new();
        }

        [Theory]
        [InlineData(-1, false, 1)]
        [InlineData(0, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateNights(int nights, bool isValid, int errorNumber)
        {
            var request = new GetRentalCalendarQuery(1, DateTime.Now, nights);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }

        [Theory]
        [InlineData(-1, false, 1)]
        [InlineData(0, false, 1)]
        [InlineData(1, true, 0)]
        public void Validator_Should_ValidateRentalId(int rentalId, bool isValid, int errorNumber)
        {
            var request = new GetRentalCalendarQuery(rentalId, DateTime.Now, 2);

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.Equal(result.IsValid, isValid);
            Assert.Equal(result.Errors.Count, errorNumber);
        }
    }
}
