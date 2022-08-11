using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
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
                Units = 2
            };

            using var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest);

            postRentalResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 02)
            };

            using var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request);

            postBooking1Response.Should().HaveStatusCode(HttpStatusCode.OK);

            var postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 05)
            };

            using var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);

            postBooking2Response.Should().HaveStatusCode(HttpStatusCode.OK);


            var postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();

            using var getCalendarResponse =
                await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5");

            getCalendarResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

            getCalendarResult.RentalId.Should().Be(postRentalResult.Id);
            getCalendarResult.Dates.Count.Should().Be(5);
            getCalendarResult.Dates[0].Bookings.Count.Should().Be(0);
            getCalendarResult.Dates[0].Date.Should().Be(new DateTime(2000, 01, 01));
          

            getCalendarResult.Dates[1].Date.Should().Be(new DateTime(2000, 01, 02));

            getCalendarResult.Dates[1].Bookings.Count.Should().Be(1);
            getCalendarResult.Dates[1].Bookings[0].Id.Should().Be(postBooking1Result.Id);

            getCalendarResult.Dates[2].Date.Should().Be(new DateTime(2000, 01, 03));
            getCalendarResult.Dates[2].Bookings.Count.Should().Be(1);
            
            getCalendarResult.Dates[3].Date.Should().Be(new DateTime(2000, 01, 04));
            getCalendarResult.Dates[3].Bookings.Count.Should().Be(0);
            
            getCalendarResult.Dates[4].Date.Should().Be(new DateTime(2000, 01, 05));
            getCalendarResult.Dates[4].Bookings.Count.Should().Be(1);
            getCalendarResult.Dates[4].Bookings[0].Id.Should().Be(postBooking2Result.Id);
            
          
        }
    }
}