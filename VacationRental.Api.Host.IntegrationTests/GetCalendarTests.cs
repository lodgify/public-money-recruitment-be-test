using System.Net.Http.Json;
using VacationRental.Api.Host.IntegrationTests;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests()
        {
            var app = new VacationRentalApplication();

            _client = app.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:9981");
        }

        [Fact(Skip = "Need to add authorization support")]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalParameters
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            var accessTokenResult = new AccessTokenDto();
            using (var accessTokenResponse = await _client.GetAsync($"/api/v1/accounts/login-guest"))
            {
                Assert.True(accessTokenResponse.IsSuccessStatusCode);
                accessTokenResult = await accessTokenResponse.Content.ReadFromJsonAsync<AccessTokenDto>();
            }

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenResult?.AccessToken}");

            BaseEntityDto postRentalResult = new RentalDto();
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            var postBooking1Request = new BookingParameters
            {
                RentalId = postRentalResult?.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 02)
            };

            BaseEntityDto postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            var postBooking2Request = new BookingParameters
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 03)
            };

            BaseEntityDto postBooking2Result = new BookingDto();
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<CalendarDto>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Length);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Length);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }
    }
}
