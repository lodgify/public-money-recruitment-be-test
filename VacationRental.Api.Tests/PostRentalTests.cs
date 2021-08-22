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
                PreparationTimeInDays = 1
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
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostUpdateRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
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
            }

            var updateRequest = new RentalBindingModel
            {
                Units = 22,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postUpdateResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals/{postResult.Id}", updateRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postUpdateResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(updateRequest.Units, getResult.Units);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateRentalUnits_ThenAPostReturnsErrorWhenThereIsConflictbooking()
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
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var update1Request = new RentalBindingModel
            {
                Units = 3,
                PreparationTimeInDays = 1
            };


            var update2Request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", update1Request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", update2Request))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateRentalPreparationTime_ThenAPostReturnsErrorWhenThereIsConflictbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
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
                Nights = 1,
                Start = new DateTime(2002, 01, 01)
            };

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 04)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var update1Request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", update1Request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            var update2Request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 4
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", update2Request))
                {
                }
            });
        }
    }
}
