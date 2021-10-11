using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Booking.DTOs;
using VacationRental.Rental.DTOs;
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
            var postRentalRequest = new AddRentalRequest
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            AddRentalResponse postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();
            }

            var postBookingRequest = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            AddRentalResponse postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<AddRentalResponse>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<GetBookingResponse>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
                Assert.Equal(1, getBookingResult.Unit);

            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAddOtherBooking_AndGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new AddRentalRequest
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            AddRentalResponse postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();
            }

            var firstBooking = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            var secondBooking = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 02)
            };



            AddRentalResponse firstBookingResponse;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", firstBooking))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                firstBookingResponse = await postBookingResponse.Content.ReadAsAsync<AddRentalResponse>();
            }
            AddRentalResponse secondBookingResponse;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", secondBooking))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                secondBookingResponse = await postBookingResponse.Content.ReadAsAsync<AddRentalResponse>();
            }


            using (var bookingResponse = await _client.GetAsync($"/api/v1/bookings/{firstBookingResponse.Id}"))
            {
                Assert.True(bookingResponse.IsSuccessStatusCode);

                var getBookingResult = await bookingResponse.Content.ReadAsAsync<GetBookingResponse>();
                Assert.Equal(firstBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(firstBooking.Nights, getBookingResult.Nights);
                Assert.Equal(firstBooking.Start, getBookingResult.Start);
                Assert.Equal(1, getBookingResult.Unit);

            }
            using (var bookingResponse = await _client.GetAsync($"/api/v1/bookings/{secondBookingResponse.Id}"))
            {
                Assert.True(bookingResponse.IsSuccessStatusCode);

                var getBookingResult = await bookingResponse.Content.ReadAsAsync<GetBookingResponse>();
                Assert.Equal(secondBooking.RentalId, getBookingResult.RentalId);
                Assert.Equal(secondBooking.Nights, getBookingResult.Nights);
                Assert.Equal(secondBooking.Start, getBookingResult.Start);
                Assert.Equal(2, getBookingResult.Unit);

            }

        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new AddRentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            AddRentalResponse postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();
            }

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new AddBookingRequest
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
            var postRentalRequest = new AddRentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            AddRentalResponse postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();
            }

            var firstSuccededBooking = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 6, 01)
            };
            var secondFailedByPreparationBooking = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = firstSuccededBooking.Start.AddDays(firstSuccededBooking.Nights)
            };
            var secondSuccededBooking = new AddBookingRequest
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
