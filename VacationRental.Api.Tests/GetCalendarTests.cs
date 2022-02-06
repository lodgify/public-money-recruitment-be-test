using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Calendar.Responses;
using VacationRental.Api.Models;
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
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2022, 02, 06),
                 Unit = 1
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 02, 06),
                Unit = 2
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-02-05&nights=7"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(7, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 02, 05), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2022, 02, 06), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count());
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 02, 07), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 02, 08), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Single(getCalendarResult.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, x => x.Unit == postBooking1Request.Unit);
                
                Assert.Equal(new DateTime(2022, 02, 09), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Equal(2,getCalendarResult.Dates[4].PreparationTimes.Count());
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == postBooking1Request.Unit);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == postBooking2Request.Unit);
                
                Assert.Equal(new DateTime(2022, 02, 10), getCalendarResult.Dates[5].Date);
                Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Equal(2,getCalendarResult.Dates[5].PreparationTimes.Count());
                Assert.Contains(getCalendarResult.Dates[5].PreparationTimes, x => x.Unit == postBooking1Request.Unit);
                Assert.Contains(getCalendarResult.Dates[5].PreparationTimes, x => x.Unit == postBooking2Request.Unit);
                
                Assert.Equal(new DateTime(2022, 02, 11), getCalendarResult.Dates[6].Date);
                Assert.Empty(getCalendarResult.Dates[6].Bookings);
                Assert.Single(getCalendarResult.Dates[6].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[6].PreparationTimes, x => x.Unit == postBooking2Request.Unit);
            }
        }
    }
}
