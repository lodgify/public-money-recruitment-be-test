using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
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

            ResourceIdViewModel postRentalResult = await _client.PostRentalAndAssertSuccess(postRentalRequest);

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2022, 01, 02)
            };

            
            var postBooking1Result = await _client.PostBookingAndAssertSuccess(postBooking1Request);
            

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 03)
            };

            

            ResourceIdViewModel postBooking2Result = await _client.PostBookingAndAssertSuccess(postBooking2Request);

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2022, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                
                Assert.Equal(new DateTime(2022, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendarWithPreparationTimes()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 3,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult = await _client.PostRentalAndAssertSuccess(postRentalRequest);

            ResourceIdViewModel postBookingResult1 = await PostBooking(postRentalResult.Id, 2, new DateTime(2022, 01, 04));
            ResourceIdViewModel postBookingResult2 = await PostBooking(postRentalResult.Id, 1, new DateTime(2022, 01, 02));
            ResourceIdViewModel postBookingResult3 = await PostBooking(postRentalResult.Id, 3, new DateTime(2022, 01, 05));
            ResourceIdViewModel postBookingResult4 = await PostBooking(postRentalResult.Id, 2, new DateTime(2022, 01, 03));

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/vacationrental/calendar?rentalId={postRentalResult.Id}&start=2022-01-04&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 01, 04), getCalendarResult.Dates[0].Date);
                Assert.Equal(2, getCalendarResult.Dates[0].Bookings.Count);
                Assert.Single(getCalendarResult.Dates[0].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => (x.Id == postBookingResult1.Id || x.Id == postBookingResult4.Id));
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => (x.Unit == 1 || x.Unit == 3));
                Assert.Contains(getCalendarResult.Dates[0].PreparationTimes, x => (x.Unit == 2));

                Assert.Equal(new DateTime(2022, 01, 05), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Single(getCalendarResult.Dates[1].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => (x.Id == postBookingResult1.Id || x.Id == postBookingResult3.Id));
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => (x.Unit == 1 || x.Unit == 2));
                Assert.Contains(getCalendarResult.Dates[1].PreparationTimes, x => x.Unit == 3);

                Assert.Equal(new DateTime(2022, 01, 06), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[2].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBookingResult3.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Unit == 2);
                Assert.Contains(getCalendarResult.Dates[2].PreparationTimes, x => (x.Unit == 1 || x.Unit == 3));

                Assert.Equal(new DateTime(2022, 01, 07), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Single(getCalendarResult.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBookingResult3.Id);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Unit == 2);
                Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, x => x.Unit == 1);

                Assert.Equal(new DateTime(2022, 01, 08), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Single(getCalendarResult.Dates[4].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == 2);
            }
        }

        private async Task<ResourceIdViewModel> PostBooking(int rentalId, int nights, DateTime startDate)
        {
            var postBooking1Request = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = nights,
                Start = startDate
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            return postBooking1Result;
        }
    }
}
