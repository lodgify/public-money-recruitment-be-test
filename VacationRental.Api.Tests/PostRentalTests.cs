using Xunit;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Models.Common;
using VacationRental.Api.Tests.Helpers;
using Code = VacationRental.Api.Tests.Helpers.Common;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;


namespace VacationRental.Api.Tests
{
    public class PostRentalTests
    {
        private readonly HttpClientHelper client;

        public PostRentalTests()
        {
            client = new HttpClientHelper(new IntegrationFixture().Client);
        }

        [Fact]
        public async Task RegisterRental_CompareRequest_To_Responce_Parameters_AwaitSuccess()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 10
            };

            var postRental = await client.PostAsync<ResourceIdViewModel>("/api/v1/rentals", request, Code.OK);
            var getRental = await client.GetAsync<RentalViewModel>($"/api/v1/rentals/{postRental.Id}", Code.OK);

            Assert.Equal(request.Units, getRental.Units);
            Assert.Equal(request.PreparationTimeInDays, getRental.PreparationTimeInDays);
        }

        [Fact]
        public async Task AttemptCreateRental_WithZeroUnits_AwaitFail()
        {
            var request = new RentalBindingModel { Units = 0 };

            var postRental = await client.PostAsync<ExceptionViewModel>("/api/v1/rentals", request, Code.Unprocessable);
            Assert.True(postRental.Message == Error.RentalUnitsZero);
        }

        [Fact]
        public async Task AttemptCreateRental_LessZeroUnits_AwaitFail()
        {
            var request = new RentalBindingModel { Units = -10 };

            var postRental = await client.PostAsync<ExceptionViewModel>("/api/v1/rentals", request, Code.Unprocessable);
            Assert.True(postRental.Message == Error.RentalUnitsZero);
        }

        [Fact]
        public async Task AttemptCreateRental_LessZeroPreparationTime_AwaitFail()
        {
            var request = new RentalBindingModel
            {
                Units = 10,
                PreparationTimeInDays = -1
            };
            var postRental = await client.PostAsync<ExceptionViewModel>("/api/v1/rentals", request, Code.Unprocessable);
            Assert.True(postRental.Message == Error.PreparationTimeLessZero);
        }
    }
}
