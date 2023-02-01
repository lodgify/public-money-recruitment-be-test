using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public sealed class PostVacationRentalTests : BaseIntegrationTests
    {
        public PostVacationRentalTests(IntegrationFixture fixture) : base(fixture)
        {
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostVacationRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new VacationRentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await Client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await Client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }
        
        [Fact]
        public async Task GivenInvalidPreparationDays_WhenPostVacationRental_ThenAGetReturnsBadRequest()
        {
            var request = new VacationRentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 0
            };

            using var actual = await Client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request);
            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }
    }
}
