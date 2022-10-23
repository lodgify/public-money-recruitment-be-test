using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_UpdateRentalDetails_ThenAPostReturnsOk200()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Unit = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getRentalDetailActual;
            using (var getRentalResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getRentalResponse.IsSuccessStatusCode);
                getRentalDetailActual = await getRentalResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            var putRentalRequest = new RentalBindingModel
            {
                Unit = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel putRentalResult;
            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putRentalResponse.IsSuccessStatusCode);
                putRentalResult = await putRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getRentalDetailExpected;
            using(var getRentalResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getRentalResponse.IsSuccessStatusCode);
                getRentalDetailExpected = await getRentalResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            Assert.True(putRentalResult.Id > 0);
            Assert.NotEqual(getRentalDetailExpected.Units, getRentalDetailActual.Units);
            Assert.NotEqual(getRentalDetailExpected.PreparationTimeInDays, getRentalDetailActual.PreparationTimeInDays);
        }

        [Fact]
        public async Task GivenCompleteRequest_UpdateRentalDetailsWithConflict_ThenAPostReturnsBadRequest()
        {
            // Create 2 rentals with same unit
            var postRentalRequest1 = new RentalBindingModel
            {
                Unit = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult1;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest1))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult1 = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postRentalRequest2 = new RentalBindingModel
            {
                Unit = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult2;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest2))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult2 = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // Create Booking for first postRentalRequest
            var postBookingRequest1 = new BookingBindingModel
            {
                RentalId = postRentalResult1.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest1))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = postRentalResult1.Id,
                Nights = 3,
                Start = new DateTime(2001, 04, 01)
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // Update rental with overlapped rentals
            var putRentalRequest = new RentalBindingModel
            {
                Unit = 2,
                PreparationTimeInDays = 3
            };

            string errorMessage;
            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult1.Id}", putRentalRequest))
            {
                Assert.False(putRentalResponse.IsSuccessStatusCode);
                errorMessage = await putRentalResponse.Content.ReadAsStringAsync();
            }

            Assert.Equal("Rent overlapped", errorMessage);
        }
    }
}
