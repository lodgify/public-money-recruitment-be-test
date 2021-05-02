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
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task RegisterRental_CompareRequest_To_Responce_Parameters_AwaitSuccess()
        {
            var request = new RentalBindingModel
            {
                Units = 25, 
                PreparationTimeInDays = 10
            };

            var postRental = await HttpClientHelper.Post<ResourceIdViewModel>("/api/v1/rentals", request, _client, Code.OK );
            var getRental = await HttpClientHelper.Get<RentalViewModel>($"/api/v1/rentals/{postRental.Id}", _client, Code.OK );
            
            Assert.Equal(request.Units, getRental.Units);
            Assert.Equal(request.PreparationTimeInDays, getRental.PreparationTimeInDays);
        }

        [Fact]
        public async Task AttemptCreateRental_WithZeroUnits_AwaitFail() 
        {
            var request = new RentalBindingModel {  Units = 0 };

            var postRental = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/rentals", request, _client, Code.Unprocessable);
            Assert.True(postRental.Message == Error.RentalUnitsZero);
        }

        [Fact]
        public async Task AttemptCreateRental_LessZeroUnits_AwaitFail()
        {
            var request = new RentalBindingModel { Units = -10 };

            var postRental = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/rentals", request, _client, Code.Unprocessable);
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
            var postRental = await HttpClientHelper.Post<ExceptionViewModel>("/api/v1/rentals", request, _client, Code.Unprocessable);
            Assert.True(postRental.Message == Error.PreparationTimeLessZero);
        }
    }
}
