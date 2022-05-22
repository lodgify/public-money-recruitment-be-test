using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PreparationTimeTests
    {
        private readonly HttpClient _client;

        public PreparationTimeTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task HandleOperations_ThenGetCalculatedCalendar()
        {
            // Scenario
            // 1- Create a Rental with 5 Units and With PrepartionTimeInDays = 1
            // 2- Add 10 Booking (each booking has 4 Nights)
            // 3- Get Calender And Assert on Bookings And PreparationTimes
            // 4- Try Update the rental With PrepartionTimeInDays = 2 => Should fails because the calender is full
            // 5- Try Update the rental with PrepartionTimeInDays = 0 => Should Success
            // 6- Get Calender And Assert that PreparationTimes is always empty
            // 7- Try Update the rental with PrepartionTimeInDays = 1 => Should Success
            // 8- Get Calender And Assert on Bookings And PreparationTimes
            
            // 1- Create a Rental with 5 Units and With PrepartionTimeInDays = 1
            var postRentalRequest = new RentalBindingModel
            {
                Units = 5,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // 2- Add 10 Booking (each booking has 4 Nights)
            List<int> bookingIds = new List<int>();
            for (int i = 0; i < 10; ++i)
            {
                var postBookingRequest = new BookingBindingModel
                {
                    RentalId = postRentalResult.Id,
                    Nights = 4,
                    Start = new DateTime(2000, 01, 01).AddDays(i)
                };

                ResourceIdViewModel postBookingResult;
                using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
                {
                    Assert.True(postBooking1Response.IsSuccessStatusCode);
                    postBookingResult = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
                    bookingIds.Add(postBookingResult.Id);
                }
            }

            // 3- Get Calender And Assert on Bookings And PreparationTimes
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=18"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);
                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                AssertonBookingAndPreparationTimes(getCalendarResult, bookingIds);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 5,
                PreparationTimeInDays = 2
            };

            // 4- Try Update the rental With PrepartionTimeInDays = 2 => Should fails because the calender is full
            using (var getupdateResponse = await _client.PutAsJsonAsync($"api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(getupdateResponse.IsSuccessStatusCode);
                var putRentalResult = await getupdateResponse.Content.ReadAsAsync<UpdateRentalResultViewModel>();
                Assert.False(putRentalResult.updated);
            };

            // 5- Try Update the rental with PrepartionTimeInDays = 0 => Should Success
            putRentalRequest.PreparationTimeInDays = 0;
            using (var getupdateResponse = await _client.PutAsJsonAsync($"api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(getupdateResponse.IsSuccessStatusCode);
                var putRentalResult = await getupdateResponse.Content.ReadAsAsync<UpdateRentalResultViewModel>();
                Assert.True(putRentalResult.updated);
            };

            // 6- Get Calender And Assert that PreparationTimes is always empty
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=18"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(18, getCalendarResult.Dates.Count);
                Assert.True(getCalendarResult.Dates.TrueForAll(d => !d.PreparationTimes.Any()));
            };

            // 7- Try Update the rental with PrepartionTimeInDays = 1 => Should Success
            putRentalRequest.PreparationTimeInDays = 1;
            using (var getupdateResponse = await _client.PutAsJsonAsync($"api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(getupdateResponse.IsSuccessStatusCode);
                var putRentalResult = await getupdateResponse.Content.ReadAsAsync<UpdateRentalResultViewModel>();
                Assert.True(putRentalResult.updated);
            };

            // 8- Get Calender And Assert on Bookings And PreparationTimes
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=18"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);
                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                AssertonBookingAndPreparationTimes(getCalendarResult, bookingIds);
            };
        }

        private static void AssertonBookingAndPreparationTimes(CalendarViewModel getCalendarResult, List<int> bookingIds)
        {
            Assert.Equal(18, getCalendarResult.Dates.Count);
            for (int i = 0; i < getCalendarResult.Dates.Count; ++i)
            {
                var ele = getCalendarResult.Dates[i];
                Assert.Equal(new DateTime(2000, 01, 01).AddDays(i), ele.Date);

                // Check The Bookings

                if (i == 0 || i == 12)
                {
                    Assert.Single(ele.Bookings);

                    Assert.Equal(bookingIds[i == 0 ? 0 : 9], ele.Bookings[0].Id);
                    Assert.Equal(i == 0 ? 1 : 5, ele.Bookings[0].Unit);
                }
                else if (i == 1 || i == 11)
                {
                    Assert.Equal(2, ele.Bookings.Count);

                    Assert.Equal(bookingIds[ i == 1 ? 0 : 8], ele.Bookings[0].Id);
                    Assert.Equal(bookingIds[ i == 1 ? 1 : 9], ele.Bookings[1].Id);

                    Assert.Equal(i == 1 ? 1 : 4, ele.Bookings[0].Unit);
                    Assert.Equal(i == 1 ? 2 : 5, ele.Bookings[1].Unit);
                }
                else if (i == 2 || i == 10)
                {
                    Assert.Equal(3, ele.Bookings.Count);

                    Assert.Equal(bookingIds[i == 2 ? 0 : 7], ele.Bookings[0].Id);
                    Assert.Equal(bookingIds[i == 2 ? 1 : 8], ele.Bookings[1].Id);
                    Assert.Equal(bookingIds[i == 2 ? 2 : 9], ele.Bookings[2].Id);

                    Assert.Equal(i == 2 ? 1 : 3, ele.Bookings[0].Unit);
                    Assert.Equal(i == 2 ? 2 : 4, ele.Bookings[1].Unit);
                    Assert.Equal(i == 2 ? 3 : 5, ele.Bookings[2].Unit);

                }
                else if (i >= 3 && i < 10)
                {
                    Assert.Equal(4, ele.Bookings.Count);

                    Assert.Equal(bookingIds[i - 3], ele.Bookings[0].Id);
                    Assert.Equal(bookingIds[i - 2], ele.Bookings[1].Id);
                    Assert.Equal(bookingIds[i - 1], ele.Bookings[2].Id);
                    Assert.Equal(bookingIds[i]    , ele.Bookings[3].Id);

                    Assert.Equal((i - 2) % 5 == 0 ? 5 : (i - 2) % 5 , ele.Bookings[0].Unit) ;
                    Assert.Equal((i - 1) % 5 == 0 ? 5 : (i - 1) % 5, ele.Bookings[1].Unit);
                    Assert.Equal( i      % 5 == 0 ? 5 : i % 5, ele.Bookings[2].Unit);
                    Assert.Equal((i + 1) % 5 == 0 ? 5 : (i + 1) % 5, ele.Bookings[3].Unit);

                }
                else if (i >= 13)
                {
                    Assert.Empty(ele.Bookings);
                }

                // Check The Prepartion Time
                if (i > 3 && i < 14)
                {
                    Assert.Single(ele.PreparationTimes);
                    Assert.Equal((i - 3) % 5 == 0 ? 5: (i - 3) % 5, ele.PreparationTimes[0].Unit);
                }
                else
                {
                    Assert.Empty(ele.PreparationTimes);
                }
            }
        }
    }
}
