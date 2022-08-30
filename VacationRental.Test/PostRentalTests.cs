using Application.Models;
using Application.Models.Rental.Requests;
using Domain.Entities;
using Xunit;

namespace VacationRental.Test
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            
            await using var application = new MinimalApisApplication();
            var client = application.CreateClient();
            
            var request = new CreateRentalRequest()
            {
                Units = 25,
                PreparationTimeInDays = 5
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<Rental>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}
