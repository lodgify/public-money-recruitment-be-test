using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Tests
{
    [TestClass]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests()
        {
            var webAppFactory = new IntegrationFixture();
            _client = webAppFactory.Client;
        }

        [TestMethod]
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
                Assert.IsTrue(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.IsTrue(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadFromJsonAsync<RentalViewModel>();
                Assert.AreEqual(request.Units, getResult.Units);
                Assert.AreEqual(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}
