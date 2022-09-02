using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Api.Tests.Common;
using VR.Application.Requests.AddBooking;
using VR.Application.Requests.AddRental;
using VR.Application.Requests.UpdateRental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class UpdateRentalTest
    {
        private readonly VRApplication _vacationRentalApplication;

        public UpdateRentalTest()
        {
            _vacationRentalApplication = new VRApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateRental_ThenAnUpdateReturnSuccessResponseWhenPreparationTimeIsIncreased()
        {
            var addRentalRequest = new AddRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            var postRentalResult = await _vacationRentalApplication.AddRentalAsync(addRentalRequest);

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 06)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking1Request);

            var postBooking2Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 10, 12)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking2Request);

            var updateRentalRequest = new UpdateRentalRequest()
            {
                Units = 2,
                PreparationTimeInDays = 4
            };

            var response = await _vacationRentalApplication.UpdateRentalAsync(postRentalResult.Id, updateRentalRequest);

            var updateRentalResponse = await response.Content.ReadFromJsonAsync<UpdateRentalResponse>();
            Assert.Equal(updateRentalRequest.Units, updateRentalResponse.Units);
            Assert.Equal(updateRentalRequest.PreparationTimeInDays, updateRentalResponse.PreparationTimeInDays);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateRental_ThenAnUpdateReturnConflictResponseWhenPreparationTimeIsIncreased()
        {
            var addRentalRequest = new AddRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            var postRentalResult = await _vacationRentalApplication.AddRentalAsync(addRentalRequest);

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 06)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking1Request);

            var postBooking2Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 10, 12)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking2Request);

            var postBooking3Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 12)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking3Request);

            var updateRentalRequest = new UpdateRentalRequest()
            {
                Units = 2,
                PreparationTimeInDays = 8
            };

            var response = await _vacationRentalApplication.UpdateRentalAsync(postRentalResult.Id, updateRentalRequest);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateRental_ThenAnUpdateReturnConflictResponseWhenUnitCoundIsDecreased()
        {
            var addRentalRequest = new AddRentalRequest
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            var postRentalResult = await _vacationRentalApplication.AddRentalAsync(addRentalRequest);

            var postBooking1Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 06)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking1Request);

            var postBooking2Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 10, 12)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking2Request);

            var postBooking3Request = new AddBookingRequest
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 10, 12)
            };

            await _vacationRentalApplication.AddBookingAsync(postBooking3Request);

            var updateRentalRequest = new UpdateRentalRequest()
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            var response = await _vacationRentalApplication.UpdateRentalAsync(postRentalResult.Id, updateRentalRequest);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
