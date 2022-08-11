using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
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

            using var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request);

            postResponse.Should().HaveStatusCode(HttpStatusCode.OK);
           
            var postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            using var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}");
            
            getResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();

            getResult.Units.Should().Be(request.Units);
        
        }

        [Fact]
        public async Task GivenIncompleteRequest_WhenPostRental_ThenAGetReturnsBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = -2
            };
            using var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request);
    
            postResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        
    }
}
