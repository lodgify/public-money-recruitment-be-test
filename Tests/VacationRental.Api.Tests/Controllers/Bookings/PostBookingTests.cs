using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Tests.ApiRoutes;
using VacationRental.Application.Bookings.Commands.PostBooking;
using VacationRental.Application.Bookings.Queries.GetBooking;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using Xunit;

namespace VacationRental.Api.Tests.Controllers.Bookings
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenOneRental_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRentalRequest))
            {
                postRentalResponse.IsSuccessStatusCode.Should().BeTrue();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBookingRequest))
            {
                postBookingResponse.IsSuccessStatusCode.Should().BeTrue();
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync(BookingApiRoute.Get(postBookingResult.Id)))
            {
                getBookingResponse.IsSuccessStatusCode.Should().BeTrue();

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                getBookingResult.RentalId.Should().Be(postBookingRequest.RentalId);
                getBookingResult.Nights.Should().Be(postBookingRequest.Nights);
                getBookingResult.Start.Should().Be(postBookingRequest.Start);
            }
        }

        [Fact]
        public async Task GivenOneRentalWithOneUnit_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRentalRequest))
            {
                postRentalResponse.IsSuccessStatusCode.Should().BeTrue();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking1Request))
            {
                postBooking1Response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            using (var postBooking2Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking2Request))
            {
                postBooking2Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task GivenOneRentalWithTwoUnits_WhenPostTwoBookingOverlapped_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRentalRequest))
            {
                postRentalResponse.IsSuccessStatusCode.Should().BeTrue();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking1Request))
            {
                postBooking1Response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            using (var postBooking2Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking2Request))
            {
                postBooking2Response.IsSuccessStatusCode.Should().BeTrue();
            }
        }

        [Fact]
        public async Task
            GivenOneRentalWithOneUnit_WhenPostThreeBookingsOneFollowAnotherOne_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRentalRequest))
            {
                postRentalResponse.IsSuccessStatusCode.Should().BeTrue();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking1Request))
            {
                postBooking1Response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 06)
            };

            using (var postBooking2Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking2Request))
            {
                postBooking2Response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postBooking3Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 04)
            };

            using (var postBooking3Response =
                await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking3Request))
            {
                postBooking3Response.IsSuccessStatusCode.Should().BeTrue();
            }
        }
    }
}
