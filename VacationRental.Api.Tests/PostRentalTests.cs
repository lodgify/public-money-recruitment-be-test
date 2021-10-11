using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenWithNoBookingsAPutReturnsUpdatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel rentalResponse;
            using (var rentalRequest = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(rentalRequest.IsSuccessStatusCode);
                rentalResponse = await rentalRequest.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var updateRequest = new RentalBindingModel
            {
                Units = 20,
                PreparationTimeInDays = 2
            };

            using (var updateResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{rentalResponse.Id}", updateRequest))
            {
                Assert.True(updateResponse.IsSuccessStatusCode);

                var getResult = await updateResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(updateRequest.Units, getResult.Units);
                Assert.Equal(updateRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }

            using (var putResponse = await _client.GetAsync($"/api/v1/rentals/{rentalResponse.Id}"))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                var getResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(updateRequest.Units, getResult.Units);
                Assert.Equal(updateRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }

        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAPutReturnsErrorWhenThereisOverbooking()
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

            var postBookings = new BookingBindingModel[] {
                new BookingBindingModel
                    {
                        RentalId = postRentalResult.Id,
                        Nights = 3,
                        Start = new DateTime(2004, 01, 01)
                    },
                new BookingBindingModel
                {
                    RentalId = postRentalResult.Id,
                    Nights = 1,
                    Start = new DateTime(2004, 01, 05)
                }
             };
            foreach (BookingBindingModel bookingPost in postBookings)
            {
                using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingPost))
                {
                    Assert.True(postBooking1Response.IsSuccessStatusCode);
                }
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_AddSomeBookings_APutReturnsUpdatedRental()
        {
            var rentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel rentalResponse;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", rentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                rentalResponse = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            var firstBooking = new BookingBindingModel
            {
                RentalId = rentalResponse.Id,
                Nights = 3,
                Start = new DateTime(2003, 01, 01)
            };

            var postBookings = new BookingBindingModel[] {
                new BookingBindingModel
                {
                    RentalId = rentalResponse.Id,
                    Nights = 1,
                    Start = firstBooking.Start.AddDays(firstBooking.Nights + rentalRequest.PreparationTimeInDays + 1)
                },
                new BookingBindingModel
                {
                    RentalId = rentalResponse.Id,
                    Nights = 1,
                    Start = firstBooking.Start.AddDays(firstBooking.Nights + (rentalRequest.PreparationTimeInDays * 2) + 2)
                }
             };
            foreach (BookingBindingModel bookingRequest in postBookings)
            {
                using (var bookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest))
                {
                    Assert.True(bookingResponse.IsSuccessStatusCode);
                }
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{rentalResponse.Id}", putRentalRequest))
                {
                    Assert.True(putRentalResponse.IsSuccessStatusCode);
                }
            });

        }

    }
}
