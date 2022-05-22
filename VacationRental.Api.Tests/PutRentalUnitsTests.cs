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
    public class PutRentalUnitsTests
    {
        private readonly HttpClient _client;
        public PutRentalUnitsTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalByDecreasingRentalUnits_ThenPutRentalReturnsErrorOverlappingBookings()
        {

            var rentalRequest = new RentalDto
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            RentalDto postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", rentalRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<RentalDto>();

                Assert.Equal(rentalRequest.PreparationTimeInDays, postResult.PreparationTimeInDays);
                Assert.Equal(rentalRequest.Units, postResult.Units);
            }

            BookingDto bookingPostResult;

            var bookingRequest_1 = new BookingDto
            {
                RentalId = 1,
                Start = new DateTime(2022, 06, 15),
                Nights = 1,
                Unit = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest_1))
            {
                Assert.True(postResponse.IsSuccessStatusCode);

                bookingPostResult = await postResponse.Content.ReadAsAsync<BookingDto>();

                Assert.Equal(bookingRequest_1.RentalId, bookingPostResult.RentalId);
                Assert.Equal(bookingRequest_1.Start, bookingPostResult.Start);
                Assert.Equal(bookingRequest_1.Unit, bookingPostResult.Unit);
                Assert.Equal(bookingRequest_1.Nights, bookingPostResult.Nights);
            }

            var bookingRequest_2 = new BookingDto
            {
                RentalId = 1,
                Start = new DateTime(2022, 06, 16),
                Nights = 1,
                Unit = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest_2))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                bookingPostResult = await postResponse.Content.ReadAsAsync<BookingDto>();
            }

            var rentalPutRequest = new RentalDto
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/1", rentalPutRequest))
            {
                Assert.True(postResponse.StatusCode == HttpStatusCode.BadRequest);
            }
        }

    }
}
