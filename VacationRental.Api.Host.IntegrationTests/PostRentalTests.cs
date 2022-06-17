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
        public async Task Rental_ShouldAddRental_ThenAGetReturnsTheCreatedRental()
        {
            // Add rental
            var rentalParameters = new RentalParameters
            {
                Units = 25,
                PreparationTimeInDays = 1
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(rentalParameters);

            // Get rental
            var getRentalResult = await _vacationRentalApplication.GetRentalAsync(rentalResult.Id);
            
            // Assert
            Assert.Equal(rentalParameters.Units, getRentalResult.Units);
        }

        [Fact]
        public async Task Rental_ShouldAddRental_ThenReturnsValidationErrorWhenThereIsUnitsZero()
        {
            // Arrange
            var rentalParameters = new RentalParameters
            {
                Units = 0,
                PreparationTimeInDays = 1
            };

            // Action & Assert
            await Assert.ThrowsAsync<ApplicationException>(async () => await _vacationRentalApplication.AddRentalAsync(rentalParameters));
        }

        [Fact]
        public async Task Rental_UpdateRentalWithDecreasingUnits_Fail()
        {
            // Arrange
            var rentalParameters = new RentalParameters {
                Units = 1,
                PreparationTimeInDays = 1
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(rentalParameters);

            // Add booking
            var bookingParameters = new BookingParameters {
                RentalId = rentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };
            var bookingResult = await _vacationRentalApplication.AddBookingAsync(bookingParameters);

            // Action & Assert
            var updateRentalParameters = new RentalParameters {
                Units = 0,
                PreparationTimeInDays = 1
            };
            await Assert.ThrowsAsync<ApplicationException>(async () => await _vacationRentalApplication.UpdateRentalAsync(rentalResult.Id, updateRentalParameters));
        }
    }
}
