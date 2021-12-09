using FluentAssertions;
using RentalSoftware.Core.Entities;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RentalSoftware.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public PostRentalTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _client = fixture.Client;
            this._testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalViewModel
            {
                Units = 5,
                PreparationTime = 1
            };

            IdentifierViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                postResponse.EnsureSuccessStatusCode();
                _testOutputHelper.WriteLine(postResponse.StatusCode.ToString());
                postResult = await postResponse.Content.ReadAsAsync<IdentifierViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                getResponse.EnsureSuccessStatusCode();

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                request.Units.Should().Be(getResult.Units);
                request.PreparationTime.Should().Be(getResult.PreparationTime);
            }
        }
    }
}
