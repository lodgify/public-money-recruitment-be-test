using Xunit;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Models.Common;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;


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
                Units = 25
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
            }
        }

        [Fact]
        public async Task AttemptCreateRental_WithZeroUnits_AwaitFail() 
        {
            var request = new RentalBindingModel
            {
                Units = 0
            };
            using (var response = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.False(response.IsSuccessStatusCode);
                Assert.True(response.StatusCode == HttpStatusCode.UnprocessableEntity);

                string respMessage = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(respMessage);

                var errorModel = JsonConvert.DeserializeObject<ExceptionViewModel>(respMessage);
                Assert.True(errorModel.Message == Error.RentalUnitsZero);
            }
        }

        [Fact]
        public async Task AttemptCreateRental_LessZeroUnits_AwaitFail()
        {
            var request = new RentalBindingModel
            {
                Units = -10
            };
            using (var response = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.False(response.IsSuccessStatusCode);
                Assert.True(response.StatusCode == HttpStatusCode.UnprocessableEntity);

                string respMessage = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(respMessage);

                var errorModel = JsonConvert.DeserializeObject<ExceptionViewModel>(respMessage);
                Assert.True(errorModel.Message == Error.RentalUnitsZero);
            }
        }

        [Fact]
        public async Task AttemptCreateRental_LessZeroPreparationTime_AwaitFail()
        {
            var request = new RentalBindingModel
            {
                Units = 10, 
                PreparationTimeInDays = -1
            };
            using (var response = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.False(response.IsSuccessStatusCode);
                Assert.True(response.StatusCode == HttpStatusCode.UnprocessableEntity);

                string respMessage = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(respMessage);

                var errorModel = JsonConvert.DeserializeObject<ExceptionViewModel>(respMessage);
                Assert.True(errorModel.Message == Error.PreparationTimeLessZero);
            }
        }
    }
}
