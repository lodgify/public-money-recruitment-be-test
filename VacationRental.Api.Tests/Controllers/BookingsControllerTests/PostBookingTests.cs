using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using Xunit;

namespace VacationRental.Api.IntegrationTests.Controllers.BookingsControllerTests
{
    [Collection("Integration")]
    public class PostBookingTests : IClassFixture<VacationRentalWebApplicationFactory>
    {
        private readonly VacationRentalApiCaller _apiCaller;

        public PostBookingTests(VacationRentalWebApplicationFactory factory)
        {
            _apiCaller = new VacationRentalApiCaller(factory);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var rental = await _apiCaller.PostRental(1, 1);
            var booking = await _apiCaller.PostBooking(rental.Id, 3, new DateTime(2002, 01, 01));

            var postBooking2Request = new CreateBookingCommand
            {
                RentalId = rental.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            using var postBooking2Response = await _apiCaller.PostAsync($"/api/v1/bookings", postBooking2Request);
            Assert.False(postBooking2Response.IsSuccessStatusCode);
        }
    }
}
