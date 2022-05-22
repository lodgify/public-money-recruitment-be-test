using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.DTOs;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalPreparationTimeTests
    {
        private readonly HttpClient _client;
        public PutRentalPreparationTimeTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalByIncreasingPreparationTime_ThenPutRentalReturnsErrorOverlappingBookings()
        {
            var rentalRequest = new RentalDto
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", rentalRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            var bookingRequest_1 = new BookingDto
            {
                RentalId = 1,
                Start= new DateTime(2022,06,15),
                Nights=1,
                Unit=1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest_1))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            var bookingRequest_2 = new BookingDto
            {
                RentalId = 1,
                Start = new DateTime(2022,06,19),
                Nights = 1,
                Unit = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest_2))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            var rentalPutRequest = new RentalDto
            {
                Units = 1,
                PreparationTimeInDays = 4
            };

            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/1", rentalPutRequest))
            {
                Assert.True(postResponse.StatusCode==HttpStatusCode.BadRequest);
            }
        }

   
    }
}
