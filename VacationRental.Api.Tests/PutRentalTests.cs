using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Bookings;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Rentals;
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
        public async Task GivenCompleteRequest_WhenPutRental_ThenReturnTheUpdatedRental()
        {
            var request = new RentalRequest
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceId postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceId>();
            }

            var updateRequest = new UpdateRentalRequest
            {
                Units = 26,
                PreparationTimeInDays = 2
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            using (var putResponse = await _client.PutAsync($"/api/v1/rentals/{postResult.Id}", httpContent))
            {
                Assert.True(putResponse.IsSuccessStatusCode);

                var putResult = await putResponse.Content.ReadAsAsync<RentalDto>();
                Assert.Equal(updateRequest.Units, putResult.Units);
                Assert.Equal(updateRequest.PreparationTimeInDays, putResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenRentalWithCompleteBookings_WhenPutRentalWithLessUnits_ThenReturnInvalidRental()
        {
            var request = new RentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceId postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceId>();
            }

            var postBookingRequest = new BookingRequest
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01),
                Units = 1
            };

            ResourceId postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceId>();
            }

            var postBookingRequest2 = new BookingRequest
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01),
                Units = 2
            };

            ResourceId postBookingResult2;
            using (var postBookingResponse2 = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse2.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse2.Content.ReadAsAsync<ResourceId>();
            }

            var updateRequest = new UpdateRentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            using (var putResponse = await _client.PutAsync($"/api/v1/rentals/{postResult.Id}", httpContent))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.Conflict, putResponse.StatusCode);                
            }
        }

        [Fact]
        public async Task GivenRentalWithDifferentBookings_WhenPutRentalWithMorePreparationTimeInDays_ThenReturnInvalidRental()
        {
            var request = new RentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceId postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceId>();
            }

            var postBookingRequest = new BookingRequest
            {
                RentalId = postResult.Id,
                Nights = 1,
                Start = new DateTime(2023, 01, 01),
                Units = 1
            };

            ResourceId postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceId>();
            }

            var postBookingRequest2 = new BookingRequest
            {
                RentalId = postResult.Id,
                Nights = 1,
                Start = new DateTime(2023, 01, 03),
                Units = 2
            };


            ResourceId postBookingResult2;
            using (var postBookingResponse2 = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse2.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse2.Content.ReadAsAsync<ResourceId>();
            }
           
          
            var updateRequest = new UpdateRentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            using (var putResponse = await _client.PutAsync($"/api/v1/rentals/{postResult.Id}", httpContent))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.Conflict, putResponse.StatusCode);
            }
        }
    }
}
