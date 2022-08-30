using System.Net.Http.Json;
using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Rental.Requests;
using Domain.Entities;
using FluentValidation;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace VacationRental.Test
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            await using var application = new MinimalApisApplication();
            var client = application.CreateClient();

            var postRentalRequest = new CreateRentalRequest()
            {
                Units = 4,
                PreparationTimeInDays = 3
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new CreateBookingRequest()
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 08, 28),
                Unit = 1
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<Booking>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            await using var application = new MinimalApisApplication();
            var client = application.CreateClient();


            var postRentalRequest = new CreateRentalRequest()
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new CreateBookingRequest()
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 08, 28)
            };

            using (var postBooking1Response = await client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new CreateBookingRequest()
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 08, 29),
                Unit = 1
            };

            using (var postBooking2Response =
                   await client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.False(postBooking2Response.IsSuccessStatusCode);
            }
        }
    }
}