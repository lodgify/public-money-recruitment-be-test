using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Tests.ApiRoutes;
using Xunit;

namespace VacationRental.Api.Tests.Controllers.Rental
{
    [Collection("Integration")]
    public class GetRentalTests
    {
        private readonly HttpClient _client;

        public GetRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenNoRental_WhenGetARental_ShouldReturnNotFoundResponse()
        {
            using (var getResponse = await _client.GetAsync(RentalApiRoute.Get(1)))
            {
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
