using System.Net.Http.Json;
using VacationRental.Api.Host.IntegrationTests;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests()
        {
            var app = new VacationRentalApplication();

            _client = app.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:9981");
        }

        [Fact(Skip = "Need to add authorization support")]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalParameters
            {
                Units = 25
            };

            BaseEntityDto postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadFromJsonAsync<RentalDto>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }
    }
}
