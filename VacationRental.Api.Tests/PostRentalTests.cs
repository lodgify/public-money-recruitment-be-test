using System;
using System.Threading.Tasks;
using VacationRental.Services.Models.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests : IClassFixture<VacationRentalWebApplicationFactory<Program>>
    {
        private readonly VacationRentalWebApplicationFactory<Program> _applicationFactory;

        public PostRentalTests(VacationRentalWebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        #region Tests

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            // ARRANGE
            var request = CreateRequest(25, 1);

            // ACT
            // ASSERT

            var postResult = await _applicationFactory.AddRentalAsync(request);
            Assert.Null(postResult.Error);

            var response = await _applicationFactory.GetRentalByAsync(postResult.Content.Id);
            Assert.Null(response.Error);
            Assert.Equal(request.Units, response.Content.Units);
        }

        [Fact]
        public async Task Rental_ShouldAddRental_ThenReturnsValidationErrorWhenThereIsUnitsZero()
        {
            // ARRANGE
            var request = CreateRequest(0, 1);

            // ACT
            // ASSERT
            await Assert.ThrowsAsync<ApplicationException>(async () => await _applicationFactory.AddRentalAsync(request));
        }

        //[Fact]
        //public async Task Rental_UpdateRentalWithDecreasingUnits_Fail()
        //{
        //    // Arrange
        //    var request = CreateRequest(1, 1);
        //    var rentalResult = await _applicationFactory.AddRentalAsync(request);

        //    // Add booking
        //    var bookingParameters = new BookingParameters
        //    {
        //        RentalId = rentalResult.Id,
        //        Nights = 3,
        //        Start = new DateTime(2002, 01, 01)
        //    };
        //    var bookingResult = await _vacationRentalApplication.AddBookingAsync(bookingParameters);

        //    // Action & Assert
        //    var updateRentalParameters = new RentalParameters
        //    {
        //        Units = 0,
        //        PreparationTimeInDays = 1
        //    };
        //    await Assert.ThrowsAsync<ApplicationException>(async () => await _vacationRentalApplication.UpdateRentalAsync(rentalResult.Id, updateRentalParameters));
        //}

        #endregion

        #region Private Methods

        private static CreateRentalRequest CreateRequest(int units, int preparationTime)
        {
            var request = new CreateRentalRequest
            {
                Units = units,
                PreparationTime = preparationTime
            };
            return request;
        }

        #endregion
    }
}
