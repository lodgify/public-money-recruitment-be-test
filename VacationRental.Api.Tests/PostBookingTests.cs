using System;
using System.Threading.Tasks;
using VacationRental.Services.Models.Booking;
using VacationRental.Services.Models.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests : IClassFixture<VacationRentalWebApplicationFactory<Program>>
    {
        private readonly VacationRentalWebApplicationFactory<Program> _applicationFactory;

        public PostBookingTests(VacationRentalWebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            // ARRANGE
            // ACT
            // ASSERT

            var postRentalRequest = new CreateRentalRequest
            {
                Units = 4,
                PreparationTime = 1
            };

            var postRentalResult = await _applicationFactory.AddRentalAsync(postRentalRequest);
            Assert.Null(postRentalResult.Error);

            var postBookingRequest = new CreateBookingRequest
            {
                 RentalId = postRentalResult.Content.Id,
                 Nights = 3,
                 Start = new DateTime(2001, 01, 01)
            };

            var postBookingResult = await _applicationFactory.AddBookingAsync(postBookingRequest);
            Assert.Null(postBookingResult.Error);
            var getBookingResult = await _applicationFactory.GetBookingAsync(postBookingResult.Content.Id);

            // ASSERT
            Assert.Null(getBookingResult.Error);
            Assert.Equal(postBookingRequest.RentalId, getBookingResult.Content.RentalId);
            Assert.Equal(postBookingRequest.Nights, getBookingResult.Content.Nights);
            Assert.Equal(postBookingRequest.Start, getBookingResult.Content.Start);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 1
            };
            var postRentalResult = await _applicationFactory.AddRentalAsync(postRentalRequest);
            Assert.Null(postRentalResult.Error);

            var postBooking1Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Content.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };
            await _applicationFactory.AddBookingAsync(postBooking1Request);

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Content.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _applicationFactory.AddBookingAsync(postBooking2Request);
            });
        }
    }
}
