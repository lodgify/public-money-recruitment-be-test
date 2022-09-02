using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Tests.Common;
using VR.Application.Queries.GetBooking;
using VR.Application.Requests.AddBooking;
using VR.Application.Requests.AddRental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly VRApplication _vacationRentalApplication;

        public PostBookingTests()
        {
            _vacationRentalApplication = new VRApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var request = new AddRentalRequest
            {
                Units = 25,
                PreparationTimeInDays = 5
            };

            var postResponse = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/rentals", request);
            Assert.True(postResponse.IsSuccessStatusCode);
            var postRentalResult = await postResponse.Content.ReadAsAsync<AddRentalResponse>();

            var postBookingRequest = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 11, 01)
            };

            var postBookingResponse = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/bookings", postBookingRequest);
            Assert.True(postBookingResponse.IsSuccessStatusCode);
            var postBookingResult = await postBookingResponse.Content.ReadAsAsync<AddBookingResponse>();

            var getBookingResponse = await _vacationRentalApplication.GetHttpRequestAsync($"/api/v1/bookings/{postBookingResult.Id}");
            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadAsAsync<GetBookingResponse>();
            Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
            Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
            Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new AddRentalRequest
            {
                Units = 1,
                PreparationTimeInDays = 5
            };

            var postRentalResponse = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/rentals", postRentalRequest);
            Assert.True(postRentalResponse.IsSuccessStatusCode);
            var postRentalResult = await postRentalResponse.Content.ReadAsAsync<AddRentalResponse>();

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 10, 01)
            };

            var postBooking1Response = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/bookings", postBooking1Request);
            Assert.True(postBooking1Response.IsSuccessStatusCode);

            var postBooking2Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 10, 02)
            };

            var postBooking2Response = await _vacationRentalApplication.PostHttpRequestAsync($"/api/v1/bookings", postBooking2Request);
            Assert.False(postBooking2Response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Conflict, postBooking2Response.StatusCode);
        }
    }
}
