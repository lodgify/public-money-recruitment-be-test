using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Tests
{
    [TestClass]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests()
        {
            var webAppFactory = new IntegrationFixture();
            _client = webAppFactory.Client;
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.IsTrue(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
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
                Assert.IsTrue(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();
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
                Assert.IsTrue(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5"))
            {
                Assert.IsTrue(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<CalendarViewModel>();
                
                Assert.AreEqual(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.AreEqual(5, getCalendarResult.Dates.Count);

                Assert.AreEqual(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.IsNull(getCalendarResult.Dates[0].Bookings);
                
                Assert.AreEqual(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.IsTrue(getCalendarResult.Dates[1].Bookings.Count == 1);

                Assert.IsNotNull(getCalendarResult.Dates[1].Bookings.Find(x => x.Id == postBooking1Result.Id));
                
                Assert.AreEqual(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equals(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.IsNotNull(getCalendarResult.Dates[2].Bookings.Find(x => x.Id == postBooking1Result.Id));
                Assert.IsNotNull(getCalendarResult.Dates[2].Bookings.Find(x => x.Id == postBooking2Result.Id));
                
                Assert.Equals(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.IsTrue(getCalendarResult.Dates[3].Bookings.Count == 1);
                Assert.IsNotNull(getCalendarResult.Dates[3].Bookings.Find(x => x.Id == postBooking2Result.Id));
                
                Assert.Equals(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.IsNotNull(getCalendarResult.Dates[4].Bookings);
            }
        }
    }
}
