using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Tests.ApiRoutes;
using VacationRental.Application.Bookings.Commands.PostBooking;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using Xunit;

namespace VacationRental.Api.Tests.Controllers
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
        public async Task GivenAExistingRental_WhenPutRental_ThenGetARentalUpdated()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRequest))
            {
                postResponse.IsSuccessStatusCode.Should().BeTrue();
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            
            var putRequest = new RentalBindingModel
            {
                Units = 5,
                PreparationTimeInDays = 3
            };
            
            using (var putResponse = await _client.PutAsJsonAsync(RentalApiRoute.Put(postResult.Id), putRequest))
            {
                putResponse.IsSuccessStatusCode.Should().BeTrue();
                await putResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync(RentalApiRoute.Get(postResult.Id)))
            {
                getResponse.IsSuccessStatusCode.Should().BeTrue();

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                getResult.Units.Should().Be(putRequest.Units);
                getResult.PreparationTimeInDays.Should().Be(putRequest.PreparationTimeInDays);
            }
        }
        
        [Fact]
        public async Task GivenAExistingRental_WhenPutRentalWithLessUnits_ThenGetABadRequest()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRequest))
            {
                postResponse.IsSuccessStatusCode.Should().BeTrue();
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking = new BookingBindingModel()
            {
                Nights = 4,
                Start = DateTime.Now.Date,
                RentalId = postResult.Id
            };
            
            using (var postBookingResponse = await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking))
            {
                postBookingResponse.IsSuccessStatusCode.Should().BeTrue();
                postResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            
            var putRequest = new RentalBindingModel
            {
                Units = 0,
                PreparationTimeInDays = 1
            };
            
            using (var putResponse = await _client.PutAsJsonAsync(RentalApiRoute.Put(postResult.Id), putRequest))
            {
                putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
