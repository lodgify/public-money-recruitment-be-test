using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBookingWithPreparationTime()
        {
            var rentalRequest = new RentalViewModel() { Units = 1, PreparationTimeInDays = 2 };
            ResourceIdViewModel postRentalResult = await _client.PostVacationRentalAndAssertSuccess(rentalRequest);

            var postBookingRequest1 = new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 01), Nights = 1 };
            var postBookingRequest2 = new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 04), Nights = 1 };

            ResourceIdViewModel postBookingResult1 = await _client.PostBookingAndAssertSuccess(postBookingRequest1);
            ResourceIdViewModel postBookingResult2 = await _client.PostBookingAndAssertSuccess(postBookingRequest2);

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult1.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest1.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest1.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest1.Start, getBookingResult.Start);
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult2.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest2.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest2.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest2.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostServeralBookings_WithAvailability_ThenShouldNotFail()
        {
            var rentalRequest = new RentalViewModel() { Units = 3, PreparationTimeInDays = 2 };
            ResourceIdViewModel postRentalResult = await _client.PostVacationRentalAndAssertSuccess(rentalRequest);

            ResourceIdViewModel postBookingResult1 = await _client.PostBookingAndAssertSuccess(new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 04), Nights = 2 });
            ResourceIdViewModel postBookingResult2 = await _client.PostBookingAndAssertSuccess(new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 02), Nights = 1 });
            ResourceIdViewModel postBookingResult3 = await _client.PostBookingAndAssertSuccess(new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 05), Nights = 3 });

            var postBookingRequest = new BookingBindingModel { RentalId = postRentalResult.Id, Start = new DateTime(2022, 01, 03), Nights = 2 };

            ResourceIdViewModel result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            Assert.NotNull(result);
        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
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
                Nights = 3,
                Start = new DateTime(2022, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 02)
            };


            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                var error = postBooking2Response.Content.ReadAsAsync<Error>();
                Assert.Equal(error.Result.message, $"Unable to book");

            }

        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbookingBecausePreparationTime()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 03)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 09)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var postBooking3Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 07)
            };


            using (var postBooking3Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking3Request))
            {
                var error = postBooking3Response.Content.ReadAsAsync<Error>();
                Assert.Equal(error.Result.message, $"Unable to book");
            }

        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenTheDateIsOfPast()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2020, 01, 03)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                var error = postBooking1Response.Content.ReadAsAsync<Error>();
                Assert.Equal(error.Result.message, $"Booking date should be future date");
            }

            

        }
    }
}
  