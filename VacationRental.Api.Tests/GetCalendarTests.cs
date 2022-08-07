using System;
using System.Net.Http;
using System.Threading.Tasks;
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

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
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
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
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
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                
                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                
                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendarWithPreparationTimes()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 01)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking3Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 05)
            };

            ResourceIdViewModel postBooking3Result;
            using (var postBooking3Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking3Request))
            {
                Assert.True(postBooking3Response.IsSuccessStatusCode);
                postBooking3Result = await postBooking3Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking4Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 06)
            };

            ResourceIdViewModel postBooking4Result;
            using (var postBooking4Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking4Request))
            {
                Assert.True(postBooking4Response.IsSuccessStatusCode);
                postBooking4Result = await postBooking4Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-01-01&nights=9"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(9, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);
                Assert.Single(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2022, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2022, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2022, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[3].PreparationTimes.Count);

                Assert.Equal(new DateTime(2022, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Single(getCalendarResult.Dates[4].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[4].Bookings, x => x.Id == postBooking3Result.Id);

                Assert.Equal(new DateTime(2022, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Contains(getCalendarResult.Dates[5].Bookings, x => x.Id == postBooking3Result.Id);
                Assert.Contains(getCalendarResult.Dates[5].Bookings, x => x.Id == postBooking4Result.Id);

                Assert.Equal(new DateTime(2022, 01, 07), getCalendarResult.Dates[6].Date);
                Assert.Contains(getCalendarResult.Dates[6].Bookings, x => x.Id == postBooking4Result.Id);
                Assert.Single(getCalendarResult.Dates[4].PreparationTimes);

                Assert.Equal(new DateTime(2022, 01, 08), getCalendarResult.Dates[7].Date);
                Assert.Equal(2, getCalendarResult.Dates[7].PreparationTimes.Count);
                Assert.Empty(getCalendarResult.Dates[7].Bookings);

                Assert.Equal(new DateTime(2022, 01, 09), getCalendarResult.Dates[8].Date);
                Assert.Single(getCalendarResult.Dates[8].PreparationTimes);
                Assert.Empty(getCalendarResult.Dates[8].Bookings);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendarAfterUpdateRental_ThenAGetReturnsTheRecalculatedCalendarWithPreparationTimes()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 01)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 02)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-01-01&nights=4"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(4, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);
                Assert.Single(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2022, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2022, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[2].PreparationTimes.Count);
            }


            //update the rental
            var putRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel putRentalResult;
            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putRentalResponse.IsSuccessStatusCode);
                putRentalResult = await putRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2022-01-01&nights=4"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(4, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2022, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);
                Assert.Single(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2022, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2022, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[2].PreparationTimes.Count);

                Assert.Equal(new DateTime(2022, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[3].PreparationTimes.Count);
            }
        }
    }
}
