using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests : BaseIntegrationTests
    {
        public GetCalendarTests(IntegrationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            const int expectedUnits = 2;

            var rentalId = await CreateRentalAsync(expectedUnits);
            var booking1Id = await CreateBookingAsync(rentalId, new DateTime(2000, 01, 02), 2);
            var booking2Id = await CreateBookingAsync(rentalId, new DateTime(2000, 01, 03), 2);
            
            using (var getCalendarResponse = await Client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(rentalId, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates.ElementAt(0).Date);
                Assert.Empty(getCalendarResult.Dates.ElementAt(0).Bookings);
                
                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates.ElementAt(1).Date);
                Assert.Single(getCalendarResult.Dates.ElementAt(1).Bookings);
                Assert.Contains(getCalendarResult.Dates.ElementAt(1).Bookings, x => x.Id == booking1Id);
                
                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates.ElementAt(2).Date);
                Assert.Equal(2, getCalendarResult.Dates.ElementAt(2).Bookings.Count);
                Assert.Contains(getCalendarResult.Dates.ElementAt(2).Bookings, x => x.Id == booking1Id);
                Assert.Contains(getCalendarResult.Dates.ElementAt(2).Bookings, x => x.Id == booking2Id);
                
                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates.ElementAt(3).Date);
                Assert.Single(getCalendarResult.Dates.ElementAt(3).Bookings);
                Assert.Contains(getCalendarResult.Dates.ElementAt(3).Bookings, x => x.Id == booking2Id);
                
                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates.ElementAt(4).Date);
                Assert.Empty(getCalendarResult.Dates.ElementAt(4).Bookings);
            }
        }
    }
}
