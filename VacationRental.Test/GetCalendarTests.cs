using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Calendar.Responses;
using Application.Models.Rental.Requests;
using Xunit;

namespace VacationRental.Test;

    [Collection("Integration")]
    public class GetCalendarTests
    {
     
        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            await using var application = new MinimalApisApplication();
            var client = application.CreateClient();
            
            var postRentalRequest = new CreateRentalRequest()
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest()
            {
                 RentalId = postRentalResult.Id,
                 Nights = 1,
                 Start = new DateTime(2022, 08, 28),
                 Unit = 1
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 08, 28),
                Unit = 2
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-08-28&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 08, 28), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2022, 08, 29), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                
                Assert.Equal(new DateTime(2022, 08, 30), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 08, 31), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2022, 09, 01), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }
    }

