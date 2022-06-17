using VacationRental.Api.Host.IntegrationTests.Common;
using VacationRental.Models.Paramaters;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly VacationRentalApplication _vacationRentalApplication;

        public PostBookingTests()
        {
            _vacationRentalApplication = new VacationRentalApplication();
        }

        [Fact]
        public async Task Booking_WhenAddBooking_ThenAGetReturnsTheCreatedBooking()
        {
            // Arrange

            // Add rental
            var rentalParameters = new RentalParameters
            {
                Units = 4,
                PreparationTimeInDays = 1
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(rentalParameters);

            // Add booking
            var bookingParameters = new BookingParameters
            {
                RentalId = rentalResult.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };
            var bookingResult = await _vacationRentalApplication.AddBookingAsync(bookingParameters);

            // Get booking
            var getBookingResult = await _vacationRentalApplication.GetBookingAsync(bookingResult.Id);
            
            // Action & Assert 
            Assert.Equal(bookingParameters.RentalId, getBookingResult.RentalId);
            Assert.Equal(bookingParameters.Nights, getBookingResult.Nights);
            Assert.Equal(bookingParameters.Start, getBookingResult.Start);
        }

        [Fact]
        public async Task Booking_ShouldAddBookingWithOneUnit_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            // Arrange

            // Add rental
            var rentalParameters = new RentalParameters
            {
                Units = 1,
                PreparationTimeInDays = 1
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(rentalParameters);

            // Add booking #1
            var firstBookingParameters = new BookingParameters
            {
                RentalId = rentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };
            var firstBookingResult = await _vacationRentalApplication.AddBookingAsync(firstBookingParameters);
           
            // Add booking #2
            var secondBookingParameters = new BookingParameters
            {
                RentalId = rentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };
            
            // Action & Assert
            await Assert.ThrowsAsync<ApplicationException>(async () => await _vacationRentalApplication.AddBookingAsync(secondBookingParameters));
        }
    }
}
