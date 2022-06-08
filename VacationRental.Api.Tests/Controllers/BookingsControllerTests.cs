using Moq;
using VacationRental.Api.Controllers;
using VacationRental.Models.Dtos;
using VacationRental.Services.Interfaces;
using Xunit;

namespace VacationRental.Api.Host.UnitTests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;

        private readonly BookingsController _controller;

        public BookingsControllerTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();

            _controller = new BookingsController(_bookingServiceMock.Object);
        }

        [Fact]
        public async Task Booking_ShouldGetBookings_Success()
        {
            // Arrange
            _bookingServiceMock.Setup(x => x.GetBookingsAsync()).ReturnsAsync(new[] { 
                new BookingDto { 
                    Id = 1,
                    Nights = 1,
                    RentalId = 1,
                    Start = DateTime.UtcNow
                } 
            });

            // Action
            var result = await _controller.GetBookingsAsync();

            // Assert
            Assert.NotNull(result);
            
            _bookingServiceMock.Verify(x => x.GetBookingsAsync(), Times.Once());
        }

        [Fact]
        public async Task Booking_ShouldGetBookingById_Success()
        {
            // Arrange
            const int bookingId = 1;

            _bookingServiceMock.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync(new BookingDto { 
                Id = 1,
                Nights = 1,
                RentalId = 1,
                Start = DateTime.UtcNow
            });

            // Action
            var result = await _controller.GetBookingByIdAsync(bookingId);

            // Assert
            Assert.NotNull(result);

            _bookingServiceMock.Verify(x => x.GetBookingByIdAsync(It.IsAny<int>()), Times.Once());
        }
    }
}
