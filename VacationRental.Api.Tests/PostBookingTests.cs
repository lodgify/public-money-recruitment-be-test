using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
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
                Units = 4,
                PreparationTimeInDays = 1
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
                 Start = new DateTime(2001, 01, 01)
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
                Assert.Equal(1, getBookingResult.Unit);

            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAddOtherBooking_AndGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var firstBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            var secondBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 02)
            };



            ResourceIdViewModel firstBookingResponse;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", firstBooking))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                firstBookingResponse = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            ResourceIdViewModel secondBookingResponse;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", secondBooking))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                secondBookingResponse = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            using (var bookingResponse = await _client.GetAsync($"/api/v1/bookings/{firstBookingResponse.Id}"))
            {
                Assert.True(bookingResponse.IsSuccessStatusCode);

                var getBookingResult = await bookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(firstBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(firstBooking.Nights, getBookingResult.Nights);
                Assert.Equal(firstBooking.Start, getBookingResult.Start);
                Assert.Equal(1, getBookingResult.Unit);

            }
            using (var bookingResponse = await _client.GetAsync($"/api/v1/bookings/{secondBookingResponse.Id}"))
            {
                Assert.True(bookingResponse.IsSuccessStatusCode);

                var getBookingResult = await bookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(secondBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(secondBooking.Nights, getBookingResult.Nights);
                Assert.Equal(secondBooking.Start, getBookingResult.Start);
                Assert.Equal(2, getBookingResult.Unit);

            }

        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
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
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsPreparingTime()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var firstSuccededBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 6, 01)
            };
            var secondFailedByPreparationBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = firstSuccededBooking.Start.AddDays(firstSuccededBooking.Nights)
            };
            var secondSuccededBooking = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = firstSuccededBooking.Start.AddDays(firstSuccededBooking.Nights + postRentalRequest.PreparationTimeInDays)
            };

            using (var succededBooking = await _client.PostAsJsonAsync($"/api/v1/bookings", firstSuccededBooking))
            {
                Assert.True(succededBooking.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var preparationErrorResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", secondFailedByPreparationBooking))
                {
                }
            });

            using (var secondSuccesResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", secondSuccededBooking))
            {
                Assert.True(secondSuccesResponse.IsSuccessStatusCode);
            }

        }

    }
}
