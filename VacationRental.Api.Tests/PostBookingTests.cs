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
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests()
        {
            var webAppFactory = new IntegrationFixture();
            _client = webAppFactory.Client;
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.IsTrue(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.IsTrue(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.IsTrue(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadFromJsonAsync<BookingViewModel>();
                Assert.AreEqual(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.AreEqual(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.AreEqual(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
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
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.IsTrue(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                    Assert.IsFalse(postBooking2Response.IsSuccessStatusCode);
                }
            });
        }
    }
}
