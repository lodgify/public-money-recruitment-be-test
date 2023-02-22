using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Models.ViewModels.Booking;
using Moq;
using Repository.Repository;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.BookingOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.BookingOperationsTests;

public class BookingCreateOperationTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;
    private readonly BookingCreateOperation _underTest;

    public BookingCreateOperationTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _rentalRepositoryMock = new Mock<IRentalRepository>();
        _underTest =
            new BookingCreateOperation(_bookingRepositoryMock.Object, _rentalRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenInvalidNights()
    {
        // Arrange
        var model = new BookingBindingViewModel
        {
            Nights = 0
        };

        // Act
        // Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(model));
    }


    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenNonExistingRentalId()
    {
        // Arrange
        var model = new BookingBindingViewModel
        {
            Nights = 1
        };
        _rentalRepositoryMock.Setup(x => x.IsExists(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _underTest.ExecuteAsync(model));
    }

    [Fact]
    public async Task ShouldThrowRentalNotAvailableExceptonWhenNoAvailableUnits()
    {
        // Arrange
        var model = new BookingBindingViewModel
        {
            Nights = 1
        };
        var rental = new RentalDto
        {
            Units = new List<int>()
        };
        _rentalRepositoryMock.Setup(x => x.IsExists(It.IsAny<int>())).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(rental);
        _bookingRepositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<BookingDto>());

        // Act
        // Assert
        await Assert.ThrowsAsync<RentalNotAvailableExcepton>(() => _underTest.ExecuteAsync(model));
    }

    [Fact]
    public async Task ShouldReturnExpectedWhenValidInput()
    {
        // Arrange
        var model = new BookingBindingViewModel
        {
            RentalId = 1,
            Nights = 1
        };
        var rental = new RentalDto
        {
            Id = model.RentalId,
            PreparationTimeInDays = 1,
            Units = new List<int>
            {
                1, 2, 3, 4
            }
        };
        var booking = new List<BookingDto>
        {
            new()
            {
                Id = 1,
                RentalId = model.RentalId,
                Unit = 1
            }
        };

        _rentalRepositoryMock.Setup(x => x.IsExists(It.IsAny<int>())).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(rental);
        _bookingRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(booking);
        _bookingRepositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(booking);

        // Act
        var result = await _underTest.ExecuteAsync(model);

        // Assert
        Assert.Equal(result.Id, booking.Last().Id + 1);
        _bookingRepositoryMock.Verify(x => x.GetAll(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()),
            Times.Once);
        _bookingRepositoryMock.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<BookingDto>()), Times.Once);
    }
}
