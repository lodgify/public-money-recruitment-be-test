using System;
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
                Units = 25
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
        public async Task GivenCompleteRequest_WhenPostRentalWithPreparationTime_ThenRentalHasPreparationTimeDefined()
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
        public async Task GivenCompleteRequest_WhenPutRentalWithNewPreparationTime_ThenMustFailWhenIsOverbooking()
        {
            var rentalCreationRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 0
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", rentalCreationRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(rentalCreationRequest.Units, getResult.Units);
                Assert.Equal(rentalCreationRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);


                var postBooking1Request = new BookingBindingModel
                {
                    RentalId = getResult.Id,
                    Nights = 3,
                    Start = new DateTime(2002, 01, 01)
                };

                using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
                {
                    Assert.True(postBooking1Response.IsSuccessStatusCode);
                }

                var postBooking2Request = new BookingBindingModel
                {
                    RentalId = getResult.Id,
                    Nights = 1,
                    Start = new DateTime(2002, 01, 04)
                };

                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                    Assert.True(postBooking2Response.IsSuccessStatusCode);
                }

            }

            var rentalUpdateRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", rentalUpdateRequest))
                {
                }
            });

        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalWithNewPreparationTime_ThenBookingCalendarMustBeRecalculated()
        {
            var rentalCreationRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", rentalCreationRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(rentalCreationRequest.Units, getResult.Units);
                Assert.Equal(rentalCreationRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);


                var postBooking1Request = new BookingBindingModel
                {
                    RentalId = getResult.Id,
                    Nights = 3,
                    Start = new DateTime(2002, 01, 01)
                };

                using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
                {
                    Assert.True(postBooking1Response.IsSuccessStatusCode);
                }

                var postBooking2Request = new BookingBindingModel
                {
                    RentalId = getResult.Id,
                    Nights = 1,
                    Start = new DateTime(2002, 01, 06)
                };

                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                    Assert.True(postBooking2Response.IsSuccessStatusCode);
                }
            }

            var rentalUpdateRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", rentalUpdateRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

        }
    }
}
