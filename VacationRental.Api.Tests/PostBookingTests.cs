using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Infrastructure.DTOs;
using Xunit;

namespace VacationRental.Api.Tests
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
            var postRentalRequest = new RentalCreateInputDTO
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            RentalCreateOutputDTO postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<RentalCreateOutputDTO>();
            }

            var postBookingRequest = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.UtcNow.Date
            };

            BookingsCreateOutputDTO postBookingResult;
            using (HttpResponseMessage postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<BookingsCreateOutputDTO>();
            }

            using var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}");
            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingDTO>();
            Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
            Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
            Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            Assert.Equal(1, getBookingResult.Unit);

            //Second booking for second unit
            var postBooking2Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.UtcNow.Date
            };

            BookingsCreateOutputDTO postBooking2Result;
            using (HttpResponseMessage postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBookingResult = await postBooking2Response.Content.ReadAsAsync<BookingsCreateOutputDTO>();
            }
            using var getBooking2Response = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}");
            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBooking2Result = await getBooking2Response.Content.ReadAsAsync<BookingDTO>();
            Assert.Equal(postBooking2Request.RentalId, getBooking2Result.RentalId);
            Assert.Equal(postBooking2Request.Nights, getBooking2Result.Nights);
            Assert.Equal(postBooking2Request.Start, getBooking2Result.Start);
            Assert.Equal(2, getBooking2Result.Unit);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalCreateInputDTO
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            RentalCreateOutputDTO postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<RentalCreateOutputDTO>();
            }

            var postBooking1Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.UtcNow.Date
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.UtcNow.Date.AddDays(1)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbookingDueToPreparationTime()
        {
            var postRentalRequest = new RentalCreateInputDTO
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            RentalCreateOutputDTO postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<RentalCreateOutputDTO>();
            }

            var postBooking1Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.UtcNow.Date
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.UtcNow.Date.AddDays(1)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);
            });
        }
    }
}
