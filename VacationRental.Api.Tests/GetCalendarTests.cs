using Xunit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Tests.Helpers;
using Code = VacationRental.Api.Tests.Helpers.Common;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;

namespace VacationRental.Api.Tests
{
    public class GetCalendarTests
    {
        private readonly HttpClientHelper client;

        public GetCalendarTests()
        {
            client = new HttpClientHelper(new IntegrationFixture().Client);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel { Units = 2 };
            var postRentalResult = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", postRentalRequest, Code.OK);


            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2030, 01, 02)
            };
            var postBooking1Result = await client.PostAsync<ResourceIdViewModel>($"/api/v1/bookings", postBooking1Request, Code.OK);


            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2030, 01, 03)
            };
            var postBooking2Result = await client.PostAsync<ResourceIdViewModel>($"/api/v1/bookings", postBooking2Request);
            string url = $"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2030-01-01&nights=5";
            CalendarViewModel getCalendarResponse = await client.GetAsync<CalendarViewModel>(url, Code.OK);

            Assert.Equal(postRentalResult.Id, getCalendarResponse.RentalId);
            Assert.Equal(5, getCalendarResponse.Dates.Count);

            Assert.Equal(new DateTime(2030, 01, 01), getCalendarResponse.Dates[0].Date);
            Assert.Empty(getCalendarResponse.Dates[0].Bookings);

            Assert.Equal(new DateTime(2030, 01, 02), getCalendarResponse.Dates[1].Date);
            Assert.Single(getCalendarResponse.Dates[1].Bookings);
            Assert.Contains(getCalendarResponse.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

            Assert.Equal(new DateTime(2030, 01, 03), getCalendarResponse.Dates[2].Date);
            Assert.Equal(2, getCalendarResponse.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResponse.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
            Assert.Contains(getCalendarResponse.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(new DateTime(2030, 01, 04), getCalendarResponse.Dates[3].Date);
            Assert.Single(getCalendarResponse.Dates[1].Bookings);
            Assert.Contains(getCalendarResponse.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(new DateTime(2030, 01, 05), getCalendarResponse.Dates[4].Date);
            Assert.Empty(getCalendarResponse.Dates[0].Bookings);
        }
    }
}
