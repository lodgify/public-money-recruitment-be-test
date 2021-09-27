using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Booking;
using VacationRental.Application.Commands.Rental;
using VacationRental.Application.Queries.Calendar.ViewModel;
using VacationRental.Application.Queries.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Given__ARentalIsCreated_And_TwoBookingsAreCreated__When_PutComplete_And_UnitsIncreased__Then_GetReturnsUpdatedRental()
        {
            var postRentalRequest = new CreateRentalRequest
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

            var postBooking1Request = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest2 = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var putRentalRequest = new UpdateRentalModel
            {
                Units = 10,
                PreparationTimeInDays = 1
            };

            using (var putHttpResponseMessage = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putHttpResponseMessage.IsSuccessStatusCode);
            }

            using (var getRentalResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getRentalResponse.IsSuccessStatusCode);

                var getBookingResult = await getRentalResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRentalRequest.PreparationTimeInDays, getBookingResult.PreparationTimeIdDays);
                Assert.Equal(putRentalRequest.Units, getBookingResult.Units);
            }
        }

        [Fact]
        public async Task Given__RentalIsCreated_AndTwoBookingWithOverlappedPeriods__When_PutComplete_And_UnitsDecreased__Then_PutReturnsError()
        {
            var postRentalRequest = new CreateRentalRequest
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

            var postBooking1Request = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var putRentalRequest = new UpdateRentalModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
                {
                    using (var putHttpResponseMessage =
                        await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                    {
                        Assert.True(putHttpResponseMessage.IsSuccessStatusCode);
                    }
                }
            );
        }

        [Fact]
        public async Task Given__RentalIsCreated_And_TwoBookingAreCreated__When_PutComplete_And_PreparationTimeIncreased__Then_GetCalendarResponseReturnsUpdatedPeriods()
        {
            var postRentalRequest = new CreateRentalRequest
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

            var start = new DateTime(2001, 1,1);
            var postBooking1Request = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = start
            };

            ResourceIdViewModel postBookingResult1;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult1 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = start
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var putRentalRequest = new UpdateRentalModel
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            using (var putHttpResponseMessage = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putHttpResponseMessage.IsSuccessStatusCode);
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={start.Year}-{start.Month}-{start.Day}&nights=7"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);
                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(7, getCalendarResult.Dates.Count);

                var index = 0;

                Assert.Equal(new DateTime(2001, 01, 01), getCalendarResult.Dates[index].Date);
                Assert.Equal(2, getCalendarResult.Dates[index].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult1.Id);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult2.Id);
                Assert.Empty(getCalendarResult.Dates[index].PreparationTimes);

                index++;

                Assert.Equal(new DateTime(2001, 01, 02), getCalendarResult.Dates[index].Date);
                Assert.Empty(getCalendarResult.Dates[index].PreparationTimes);
                Assert.Equal(2, getCalendarResult.Dates[index].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult1.Id);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult2.Id);
                

                index++;

                Assert.Equal(new DateTime(2001, 01, 03), getCalendarResult.Dates[index].Date);
                Assert.Equal(2, getCalendarResult.Dates[index].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult1.Id);
                Assert.Contains(getCalendarResult.Dates[index].Bookings, x => x.Id == postBookingResult2.Id);
                Assert.Empty(getCalendarResult.Dates[index].PreparationTimes);

                index++;

                Assert.Equal(new DateTime(2001, 01, 04), getCalendarResult.Dates[index].Date);
                Assert.Empty(getCalendarResult.Dates[index].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[index].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 2);

                index++;

                Assert.Equal(new DateTime(2001, 01, 05), getCalendarResult.Dates[index].Date);
                Assert.Empty(getCalendarResult.Dates[index].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[index].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 2);

                index++;

                Assert.Equal(new DateTime(2001, 01, 06), getCalendarResult.Dates[index].Date);
                Assert.Empty(getCalendarResult.Dates[index].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[index].PreparationTimes.Count);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[index].PreparationTimes, x => x.Unit == 2);

                index++;

                Assert.Equal(new DateTime(2001, 01, 07), getCalendarResult.Dates[index].Date);
                Assert.Empty(getCalendarResult.Dates[index].Bookings);
                Assert.Empty(getCalendarResult.Dates[index].PreparationTimes);
            }
        }
    }
}
