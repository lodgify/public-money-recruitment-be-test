using RentalSoftware.Core.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RentalSoftware.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalViewModel
            {
                Units = 4
            };

            IdentifierViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<IdentifierViewModel>();
            }

            var postBookingRequest = new BookingViewModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            IdentifierViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<IdentifierViewModel>();
            }

            using var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}");

            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
            Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
            Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
            Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalViewModel
            {
                Units = 1
            };

            IdentifierViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<IdentifierViewModel>();
            }

            var postBooking1Request = new BookingViewModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postRentalResult = await postBooking1Response.Content.ReadAsAsync<IdentifierViewModel>();
                Assert.True(postRentalResult.Id > 0);
            }

            var postBooking2Request = new BookingViewModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);
            });
        }
    }
}
