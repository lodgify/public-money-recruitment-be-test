using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Tests.ApiRoutes;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using Xunit;

namespace VacationRental.Api.Tests.Controllers
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
            using (var postResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), request))
            {
                postResponse.IsSuccessStatusCode.Should().BeTrue();
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync(RentalApiRoute.Get(postResult.Id)))
            {
                getResponse.IsSuccessStatusCode.Should().BeTrue();

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                getResult.Units.Should().Be(request.Units);
            }
        }
    }
}
