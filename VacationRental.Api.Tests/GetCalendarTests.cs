using System;
using System.Linq;
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
            var postRentalRequest = new RentalBindingModel
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
                Start = new DateTime(2000, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=8"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                var preparationBooking1 = getCalendarResult.Dates[2].Bookings.Single(y => y.Id == postBooking1Result.Id);
                var preparationBooking2 = getCalendarResult.Dates[3].Bookings.Single(y => y.Id == postBooking2Result.Id);

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(8, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Single(getCalendarResult.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, x => x.Unit == preparationBooking1.Unit);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[4].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == preparationBooking1.Unit);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == preparationBooking2.Unit);

                Assert.Equal(new DateTime(2000, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[5].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[5].PreparationTimes, x => x.Unit == preparationBooking1.Unit);
                Assert.Contains(getCalendarResult.Dates[5].PreparationTimes, x => x.Unit == preparationBooking2.Unit);

                Assert.Equal(new DateTime(2000, 01, 07), getCalendarResult.Dates[6].Date);
                Assert.Single(getCalendarResult.Dates[6].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[6].PreparationTimes, x => x.Unit == preparationBooking2.Unit);

                Assert.Equal(new DateTime(2000, 01, 08), getCalendarResult.Dates[7].Date);
                Assert.Empty(getCalendarResult.Dates[7].Bookings);
                Assert.Empty(getCalendarResult.Dates[7].PreparationTimes);

                //test if bookings are in same unit
                var allBooking1Units = getCalendarResult.Dates.SelectMany(x => x.Bookings.Where(y => y.Id == postBooking1Result.Id)).Select(x => x.Unit);
                var allBooking2Units = getCalendarResult.Dates.SelectMany(x => x.Bookings.Where(y => y.Id == postBooking2Result.Id)).Select(x => x.Unit);
                Assert.False(allBooking1Units.Distinct().Skip(1).Any());
                Assert.False(allBooking2Units.Distinct().Skip(1).Any());
            }
        }
    }
}
