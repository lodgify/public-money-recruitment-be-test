using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Tests
{
    [TestClass]
    public class PutRentalTest
    {
        private readonly HttpClient _client;

        public PutRentalTest()
        {
            var webAppFactory = new IntegrationFixture();
            _client = webAppFactory.Client;
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenPutRentals_ThenAPutReturnsErrorWhenThereIsOverlappingDueIncreaseOfTheLengthOfPreparationTime()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.IsTrue(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 10, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.IsTrue(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 10, 04)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.IsTrue(postBooking2Response.IsSuccessStatusCode);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
            {
                using (var postRental2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                {
                    Assert.IsFalse(postRental2Response.IsSuccessStatusCode);
                }
            });
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenPutRentals_ThenAPutReturnsErrorWhenThereIsOverlappingDueDecreaseOfTheNumberOfUnits()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.IsTrue(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 10, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.IsTrue(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 10, 04)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.IsTrue(postBooking2Response.IsSuccessStatusCode);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
            {
                using (var postRental2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                {
                    Assert.IsFalse(postRental2Response.IsSuccessStatusCode);
                }
            });

        }

    }
}
