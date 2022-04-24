using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Infrastructure.DTOs;
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
            var request = new RentalCreateInputDTO
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            RentalCreateOutputDTO postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<RentalCreateOutputDTO>();
            }

            using var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);

            var getResult = await getResponse.Content.ReadAsAsync<RentalDTO>();
            Assert.Equal(request.Units, getResult.Units);
        }
    }
}
