using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Rental;
using VacationRental.Application.Queries.Rental;
using VacationRental.Domain.Exceptions;
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

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeIdDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest__When_PostRental_And_RequestIsInvalid__ThenThePostReturnsError()
        {
            var request = new CreateRentalRequest
            {
                Units = 0,
                PreparationTimeInDays = 1
            };

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

                var errorMessage = await postResponse.Content.ReadAsStringAsync();
                Assert.Equal(new UnitsLessThanOneException().Message, errorMessage);
            }
        }
    }
}
