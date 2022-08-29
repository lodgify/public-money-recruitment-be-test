using System;
using System.Threading.Tasks;
using VacationRental.Services.Models.Rental;
using Xunit;

namespace VacationRental.Api.Tests.GiveRentalsController
{
    [Collection("Integration")]
    public partial class When : IClassFixture<VacationRentalWebApplicationFactory<Program>>
    {
        private readonly VacationRentalWebApplicationFactory<Program> _applicationFactory;

        public When(VacationRentalWebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            // ARRANGE
            var request = new CreateRentalRequest
            {
                Units = 25
            };

            // ACT
            var response = await _applicationFactory.AddRentalAsync(request);

            // ASSERT
            Assert.Null(response.Error);
            Assert.NotNull(response.Content);
            var content = response.Content;
            Assert.Equal(request.Units, content.Units);
        }
    }
}
