using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Bookings.Models;
using Xunit;

namespace VacationRental.Api.IntegrationTests.Controllers.BookingsControllerTests
{
    [Collection("Integration")]
    public class GetBookingTests : IClassFixture<VacationRentalWebApplicationFactory>
    {
        private readonly VacationRentalApiCaller _apiCaller;

        public GetBookingTests(VacationRentalWebApplicationFactory factory)
        {
            _apiCaller = new VacationRentalApiCaller(factory);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var rental = await _apiCaller.PostRental(4, 1);
            var booking = await _apiCaller.PostBooking(rental.Id, 3, new DateTime(2001, 01, 01));

            using var getBookingResponse = await _apiCaller.GetAsync($"/api/v1/bookings/{booking.Id}");
            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
            Assert.Equal(rental.Id, getBookingResult.RentalId);
            Assert.Equal(3, getBookingResult.Nights);
            Assert.Equal(new DateTime(2001, 01, 01), getBookingResult.Start);
        }
    }
}
