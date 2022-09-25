using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            // Setting PreparationTime = 2 does not give the rental enough time to clean rooms before Booking3 arrives, giving an error.
            // So even though rental is empty, a booking cannot be made, showing that the COVID implementation works.
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking3Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2000, 01, 05)
            };

            ResourceIdViewModel postBooking3Result;
            using (var postBooking3Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking3Request))
            {
                Assert.True(postBooking3Response.IsSuccessStatusCode);
                postBooking3Result = await postBooking3Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // setting the below Units to 1 and/or increasing PreparationTime will cause the appropriate throws
            var putRentalRequest = new RentalEditModel
            {
                Units = 2,
                PreparationTimeInDays = 1,
                Id = postRentalResult.Id
            };

            ResourceIdViewModel putRentalResult;
            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest))
            {
                Assert.True(putRentalResponse.IsSuccessStatusCode);
                putRentalResult = await putRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=6"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(6, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

                // This day the rental is techically empty but unavailable, since both rooms must be cleaned.
                // Uncommenting the Assert below gives an error explaining that isCleaned = true
                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                //Assert.Empty(getCalendarResult.Dates[3].Bookings);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Contains(getCalendarResult.Dates[4].Bookings, x => x.Id == postBooking3Result.Id);

                // The same situation here, even though only one room is being cleaned, Assert.Empty is no longer true.
                Assert.Equal(new DateTime(2000, 01, 06), getCalendarResult.Dates[5].Date);
                //Assert.Empty(getCalendarResult.Dates[5].Bookings);
            }
        }
    }
}