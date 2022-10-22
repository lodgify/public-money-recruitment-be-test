using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_UpdateRentalDetails_ThenAPostReturnsTrue()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getRentalDetailActual;
            using (var getRentalResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getRentalResponse.IsSuccessStatusCode);
                getRentalDetailActual = await getRentalResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel putRentalResult;
            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putRentalResponse.IsSuccessStatusCode);
                putRentalResult = await putRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            RentalViewModel getRentalDetailExpected;
            using(var getRentalResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getRentalResponse.IsSuccessStatusCode);
                getRentalDetailExpected = await getRentalResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            Assert.True(putRentalResult.Id > 0);
            Assert.NotEqual(getRentalDetailExpected.Units, getRentalDetailActual.Units);
            Assert.NotEqual(getRentalDetailExpected.PreparationTimeInDays, getRentalDetailActual.PreparationTimeInDays);
        }
    }
}
