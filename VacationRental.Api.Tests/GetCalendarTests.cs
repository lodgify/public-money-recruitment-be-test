﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Infrastructure.DTOs;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalCreateInputDTO
            {
                Units = 2,
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
                Nights = 2,
                Start = DateTime.UtcNow.AddDays(1),
            };

            BookingsCreateOutputDTO postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<BookingsCreateOutputDTO>();
            }

            var postBooking2Request = new BookingsCreateInputDTO
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.UtcNow.Date.AddDays(2)
            };

            BookingsCreateOutputDTO postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<BookingsCreateOutputDTO>();
            }

            using var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={DateTime.UtcNow:yyyy-MM-dd}&nights=5");
            Assert.True(getCalendarResponse.IsSuccessStatusCode);

            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarDTO>();

            Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
            Assert.Equal(5, getCalendarResult.Dates.Count);

            Assert.Equal(DateTime.UtcNow.Date, getCalendarResult.Dates[0].Date);
            Assert.Empty(getCalendarResult.Dates[0].Bookings);

            Assert.Equal(DateTime.UtcNow.Date.AddDays(1), getCalendarResult.Dates[1].Date);
            Assert.Single(getCalendarResult.Dates[1].Bookings);
            Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

            Assert.Equal(DateTime.UtcNow.Date.AddDays(2), getCalendarResult.Dates[2].Date);
            Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(DateTime.UtcNow.Date.AddDays(3), getCalendarResult.Dates[3].Date);
            Assert.Single(getCalendarResult.Dates[3].Bookings);
            Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(DateTime.UtcNow.Date.AddDays(4), getCalendarResult.Dates[4].Date);
            Assert.Empty(getCalendarResult.Dates[4].Bookings);
        }
    }
}
