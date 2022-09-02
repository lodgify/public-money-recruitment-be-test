using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Tests.Common;
using VR.Application.Queries.GetCalendar;
using VR.Application.Requests.AddBooking;
using VR.Application.Requests.AddRental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly VRApplication _vacationRentalApplication;

        public GetCalendarTests()
        {
            _vacationRentalApplication = new VRApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new AddRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 5
            };

            var postRentalResponse = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/rentals", postRentalRequest);
            Assert.True(postRentalResponse.IsSuccessStatusCode);
            var postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 02)
            };

            var postBooking1Response = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/bookings", postBooking1Request);
            Assert.True(postBooking1Response.IsSuccessStatusCode);
            var postBooking1Result = await postBooking1Response.Content.ReadAsAsync<AddBookingResponse>();

            var postBooking2Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 03)
            };

            var postBooking2Response = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/bookings", postBooking2Request);
            Assert.True(postBooking2Response.IsSuccessStatusCode);
            var postBooking2Result = await postBooking2Response.Content.ReadAsAsync<AddBookingResponse>();

            var getCalendarResponse = await _vacationRentalApplication.GetHttpRequestAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-10-01&nights=5");
            Assert.True(getCalendarResponse.IsSuccessStatusCode);

            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<GetCalendarResponse>();

            Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
            Assert.Equal(5, getCalendarResult.Dates.Count);

            Assert.Equal(new DateTime(2022, 10, 01), getCalendarResult.Dates[0].Date);
            Assert.Empty(getCalendarResult.Dates[0].Bookings);

            Assert.Equal(new DateTime(2022, 10, 02), getCalendarResult.Dates[1].Date);
            Assert.Single(getCalendarResult.Dates[1].Bookings);
            Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

            Assert.Equal(new DateTime(2022, 10, 03), getCalendarResult.Dates[2].Date);
            Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(new DateTime(2022, 10, 04), getCalendarResult.Dates[3].Date);
            Assert.Single(getCalendarResult.Dates[3].Bookings);
            Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(new DateTime(2022, 10, 05), getCalendarResult.Dates[4].Date);
            Assert.Empty(getCalendarResult.Dates[4].Bookings);
        }
    }
}
