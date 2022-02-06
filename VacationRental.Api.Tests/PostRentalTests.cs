using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Models;
using Domain.DAL.Models;
using FluentValidation;
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
            var request = new CreateRentalRequest
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

                var getResult = await getResponse.Content.ReadAsAsync<Rental>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAPostReturnsErrorWhenPreparationTimeInDaysIsNegative()
        {
            var request = new CreateRentalRequest
            {
                Units = 25,
                PreparationTimeInDays = -1
            };
            
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                using (var postrentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
                {
                }
            });
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAPostReturnsErrorWhenUnitsIsNegative()
        {
            var request = new CreateRentalRequest
            {
                Units = -25,
                PreparationTimeInDays = 1
            };
            
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                using (var postrentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
                {
                }
            });
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAPostReturnsErrorWhenUnitsIsAndPreparationTimeInDaysNegative()
        {
            var request = new CreateRentalRequest
            {
                Units = -25,
                PreparationTimeInDays = -1
            };
            
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                using (var postrentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
                {
                }
            });
        }
    }
}
