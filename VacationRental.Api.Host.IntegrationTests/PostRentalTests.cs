using VacationRental.Api.Host.IntegrationTests.Common;
using VacationRental.Models.Paramaters;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly VacationRentalApplication _vacationRentalApplication;

        public PostRentalTests()
        {
            _vacationRentalApplication = new VacationRentalApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            // Get access token
            var guestTokenResult = await _vacationRentalApplication.GetGuestTokenAsync();
            
            // Add rental
            var rentalParameters = new RentalParameters
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(guestTokenResult.AccessToken!, rentalParameters);

            // Get rental
            var getRentalResult = await _vacationRentalApplication.GetRentalAsync(guestTokenResult.AccessToken!, rentalResult.Id);
            
            // Assert
            Assert.Equal(rentalParameters.Units, getRentalResult.Units);
        }
    }
}
