using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Calendars.Models;
using Xunit;

namespace VacationRental.Api.IntegrationTests.Controllers.CalendarControllerTests
{
    [Collection("Integration")]
    public class GetCalendarTests : IClassFixture<VacationRentalWebApplicationFactory>
    {
        private readonly VacationRentalApiCaller _apiCaller;

        public GetCalendarTests(VacationRentalWebApplicationFactory factory)
        {
            _apiCaller = new VacationRentalApiCaller(factory);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var rental = await _apiCaller.PostRental(2, 1);
            var booking1 = await _apiCaller.PostBooking(rental.Id, 2, new DateTime(2000, 01, 02));
            var booking2 = await _apiCaller.PostBooking(rental.Id, 2, new DateTime(2000, 01, 03));

            using var getCalendarResponse = await _apiCaller.GetAsync($"/api/v1/calendar?rentalId={rental.Id}&start=2000-01-01&nights=5");

            Assert.True(getCalendarResponse.IsSuccessStatusCode);

            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

            Assert.Equal(rental.Id, getCalendarResult.RentalId);
            Assert.Equal(5, getCalendarResult.Dates.Count);

            Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
            Assert.Empty(getCalendarResult.Dates[0].Bookings);

            Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
            Assert.Single(getCalendarResult.Dates[1].Bookings);
            Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == booking1.Id);

            Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
            Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == booking1.Id);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == booking2.Id);

            Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
            Assert.Single(getCalendarResult.Dates[3].Bookings);
            Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == booking2.Id);

            Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
            Assert.Empty(getCalendarResult.Dates[4].Bookings);
        }
    }
}
