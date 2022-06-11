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

        [Theory]
        [InlineData(1, 0)]
        [InlineData(25, 20)]
        [InlineData(10, 1)]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental(int units, int preparationTimeInDays)
        {
            var request = new RentalBindingModel
            {
                Units = units,
                PreparationTimeInDays = preparationTimeInDays
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

                Assert.Equal(units, getResult.Units);
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(preparationTimeInDays, getResult.PreparationTimeInDays);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}
