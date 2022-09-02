using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Tests.Common;
using VR.Application.Queries.GetRental;
using VR.Application.Requests.AddRental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly VRApplication _vacationRentalApplication;

        public PostRentalTests()
        {
            _vacationRentalApplication = new VRApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new AddRentalRequest
            {
                Units = 25,
                PreparationTimeInDays = 5
            };

            var postResponse = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/rentals", request);
            Assert.True(postResponse.IsSuccessStatusCode);
            var postResult = await postResponse.Content.ReadAsAsync<AddRentalResponse>();

            var getResponse = await _vacationRentalApplication.GetHttpRequestAsync($"/api/v1/rentals/{postResult.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);

            var getResult = await getResponse.Content.ReadAsAsync<GetRentalResponse>();
            Assert.Equal(request.Units, getResult.Units);
            Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
        }
    }
}
