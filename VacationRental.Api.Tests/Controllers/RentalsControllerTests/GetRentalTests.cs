using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Rentals.Models;
using Xunit;

namespace VacationRental.Api.IntegrationTests.Controllers.RentalsControllerTests
{
    [Collection("Integration")]
    public class GetRentalTests : IClassFixture<VacationRentalWebApplicationFactory>
    {
        private readonly VacationRentalApiCaller _apiCaller;

        public GetRentalTests(VacationRentalWebApplicationFactory factory)
        {
            _apiCaller = new VacationRentalApiCaller(factory);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var rental = await _apiCaller.PostRental(2, 1);

            using var getResponse = await _apiCaller.GetAsync($"/api/v1/rentals/{rental.Id}");

            Assert.True(getResponse.IsSuccessStatusCode);
            var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
            Assert.Equal(2, getResult.Units);
        }
    }
}
