using Moq;
using VacationRental.Api.Controllers;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Exceptions;
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

        [Fact]
        public async Task Booking_ShouldGetBookingWithNotFoundId_Fail()
        {
            // Arrange
            const int bookingId = 1;

            _bookingServiceMock.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ThrowsAsync(new BookingNotFoundException($"{nameof(Booking)} with Id: {bookingId} not found."));

            // Action & Assert
            await Assert.ThrowsAsync<BookingNotFoundException>(async () => await _controller.GetBookingByIdAsync(bookingId));
        }

        [Fact]
        public async Task Booking_ShouldAddBooking_Success()
        {
            // Arrange
            var parameters = new Models.Paramaters.BookingParameters {
                RentalId = 1,
                Start = DateTime.UtcNow,
                Nights = 1
            };
            
            _bookingServiceMock.Setup(x => x.AddBookingAsync(It.IsAny<Models.Paramaters.BookingParameters>())).ReturnsAsync(new BaseEntityDto {
                Id = 1 
            });

            // Action
            var result = await _controller.AddBookingAsync(parameters);

            // Assert
            Assert.NotNull(result);

            _bookingServiceMock.Verify(x => x.AddBookingAsync(It.IsAny<Models.Paramaters.BookingParameters>()), Times.Once());
        }

        [Fact]
        public async Task Booking_ShouldAddBookingWihoutRental_Fail()
        {
            // Arrange
            const int rentalId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = rentalId,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.AddBookingAsync(It.IsAny<Models.Paramaters.BookingParameters>())).ThrowsAsync(new RentalNotFoundException($"{nameof(Rental)} with Id: {rentalId} not found."));

            // Action & Assert
            await Assert.ThrowsAsync<RentalNotFoundException>(async () => await _controller.AddBookingAsync(parameters));
        }

        [Fact]
        public async Task Booking_ShouldAddBookingNoAvailable_Fail()
        {
            // Arrange
            const int rentalId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = rentalId,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.AddBookingAsync(It.IsAny<Models.Paramaters.BookingParameters>())).ThrowsAsync(new BookingInvalidException($"{nameof(Booking)} not available."));

            // Action & Assert
            await Assert.ThrowsAsync<BookingInvalidException>(async () => await _controller.AddBookingAsync(parameters));
        }

        [Fact]
        public async Task Boking_ShouldUpdateBooking_Success()
        {
            // Arrange
            const int bookingId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = 1,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.UpdateBookingAsync(It.IsAny<int>(), It.IsAny<Models.Paramaters.BookingParameters>())).Returns(Task.FromResult(true));

            // Action
            var result = await _controller.UpdateBookingAsync(bookingId, parameters);

            // Assert
            Assert.NotNull(result);

            _bookingServiceMock.Verify(x => x.UpdateBookingAsync(It.IsAny<int>(), It.IsAny<Models.Paramaters.BookingParameters>()), Times.Once());
        }

        [Fact]
        public async Task Boking_ShouldUpdateBookingByNotFoundId_Fail()
        {
            // Arrange
            const int bookingId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = 1,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.UpdateBookingAsync(It.IsAny<int>(), It.IsAny<Models.Paramaters.BookingParameters>())).ThrowsAsync(new BookingNotFoundException($"{nameof(Booking)} with Id: {bookingId} not found."));

            // Action & Assert
            await Assert.ThrowsAsync<BookingNotFoundException>(async () => await _controller.UpdateBookingAsync(bookingId, parameters));
        }

        [Fact]
        public async Task Booking_ShouldUpdateBookingWihoutRental_Fail()
        {
            // Arrange
            const int rentalId = 1;
            const int bookingId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = rentalId,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.UpdateBookingAsync(It.IsAny<int>(), It.IsAny<Models.Paramaters.BookingParameters>())).ThrowsAsync(new RentalNotFoundException($"{nameof(Rental)} with Id: {rentalId} not found."));

            // Action & Assert
            await Assert.ThrowsAsync<RentalNotFoundException>(async () => await _controller.UpdateBookingAsync(bookingId, parameters));
        }

        [Fact]
        public async Task Booking_ShouldUpdateBookingNoAvailable_Fail()
        {
            // Arrange
            const int rentalId = 1;
            const int bookingId = 1;

            var parameters = new Models.Paramaters.BookingParameters
            {
                RentalId = rentalId,
                Start = DateTime.UtcNow,
                Nights = 1
            };

            _bookingServiceMock.Setup(x => x.UpdateBookingAsync(It.IsAny<int>(), It.IsAny<Models.Paramaters.BookingParameters>())).ThrowsAsync(new BookingInvalidException($"{nameof(Booking)} not available."));

            // Action & Assert
            await Assert.ThrowsAsync<BookingInvalidException>(async () => await _controller.UpdateBookingAsync(bookingId, parameters));
        }

        [Fact]
        public async Task Boking_ShouldDeleteBooking_Success()
        {
            // Arrange
            const int bookingId = 1;

            _bookingServiceMock.Setup(x => x.DeleteBookingAsync(It.IsAny<int>())).Returns(Task.FromResult(true));

            // Action
            var result = await _controller.DeleteBookingAsync(bookingId);

            // Assert
            Assert.NotNull(result);

            _bookingServiceMock.Verify(x => x.DeleteBookingAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task Booking_ShouldDeleteBookingWihoutRental_Fail()
        {
            // Arrange
            const int rentalId = 1;
            const int bookingId = 1;

            _bookingServiceMock.Setup(x => x.DeleteBookingAsync(It.IsAny<int>())).ThrowsAsync(new RentalNotFoundException($"{nameof(Rental)} with Id: {rentalId} not found."));

            // Action & Assert
            await Assert.ThrowsAsync<RentalNotFoundException>(async () => await _controller.DeleteBookingAsync(bookingId));
        }
    }
}
