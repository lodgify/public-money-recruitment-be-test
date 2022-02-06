using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Rental.Requests;
using FluentValidation;
using VacationRental.Api.Models;
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
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnSuccessWhenPreparationTimeIdDaysIsIncreased()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 02, 06),
                Unit = 1
            };
            
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 02, 12),
                Unit = 1
            };
            
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }
            
            var putRentalRequest = new UpdateRentalRequest()
            {
                Id = postRentalResult.Id,
                Units = 2,
                PreparationTimeInDays = 4
            };
            
            using (var postrentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest))
            {
                Assert.True(postrentalResponse.IsSuccessStatusCode);
            }
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnSuccessWhenAmountOfUnitsIsIncreased()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 02, 06),
                Unit = 1
            };
            
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 02, 11),
                Unit = 1
            };
            
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }
            
            var putRentalRequest = new UpdateRentalRequest()
            {
                Id = postRentalResult.Id,
                Units = 3,
                PreparationTimeInDays = 3
            };
            
            using (var postrentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest))
            {
                Assert.True(postrentalResponse.IsSuccessStatusCode);
            }
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnsErrorWhenOverlappingBecauseOfPreparationTimeIdDaysIsIncreased()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 02, 06),
                Unit = 1
            };
            
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 02, 11),
                Unit = 1
            };
            
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }
            
            var putRentalRequest = new UpdateRentalRequest()
            {
                Id = postRentalResult.Id,
                Units = 2,
                PreparationTimeInDays = 4
            };
            
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                using (var postrentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest))
                {
                }
            });
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnsErrorWhenOverlappingBecauseOfAmountOfUnitsIsDecreased()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 02, 06),
                Unit = 1
            };
            
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 02, 06),
                Unit = 2
            };
            
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }
            
            var putRentalRequest = new UpdateRentalRequest()
            {
                Id = postRentalResult.Id,
                Units = 1,
                PreparationTimeInDays = 3
            };
            
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                using (var postrentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest))
                {
                }
            });
        }
    }
}