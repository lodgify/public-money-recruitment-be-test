using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests : BaseIntegrationTests
    {
        public PostBookingTests(IntegrationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            const int expectedUnits = 4;

            var rentalId = await CreateRentalAsync(expectedUnits);
            var bookingId = await CreateBookingAsync(rentalId, new DateTime(2001, 01, 01), 3);

            using (var getBookingResponse = await Client.GetAsync($"/api/v1/bookings/{bookingId}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(rentalId, getBookingResult.RentalId);
                Assert.Equal(3, getBookingResult.Nights);
                Assert.Equal(new DateTime(2001, 01, 01), getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            const int expectedUnits = 1;

            var rentalId = await CreateRentalAsync(expectedUnits);

            await CreateBookingAsync(rentalId, new DateTime(2001, 01, 01), 2);
            
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await CreateBookingAsync(rentalId, new DateTime(2001, 01, 02), 2);
            });
        }
    }
}
