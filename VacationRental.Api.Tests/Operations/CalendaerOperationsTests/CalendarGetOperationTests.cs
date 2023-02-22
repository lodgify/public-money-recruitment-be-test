using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Moq;
using Repository.Repository;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.CalendarOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.CalendaerOperationsTests;

public class CalendarGetOperationTests
{
    private Mock<IBookingRepository> _bookingRepositoryMock;
    private Mock<IRentalRepository> _rentalRepositoryMock;
    private CalendarGetOperation _underTest;

    public CalendarGetOperationTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _rentalRepositoryMock = new Mock<IRentalRepository>();
        _underTest = new CalendarGetOperation(_bookingRepositoryMock.Object, _rentalRepositoryMock.Object);
    }


    [Fact]
    public async Task ThrowsValidationExceptionWhenInvalidRentalId()
    {
        // Arrange
        var rentalId = 0;
        var start = new DateTime(2023, 02, 23);
        var nights = 3;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(rentalId, start, nights));
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenInvalidNights()
    {
        // Arrange
        var rentalId = 1;
        var start = new DateTime(2023, 02, 23);
        var nights = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(rentalId, start, nights));
    }

    [Fact]
    public async Task ThrowsNotFoundExceptionWhenRentalNotFound()
    {
        // Arrange
        var rentalId = 1;
        var start = new DateTime(2023, 02, 23);
        var nights = 1;

        _rentalRepositoryMock.Setup(x => x.IsExists(rentalId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _underTest.ExecuteAsync(rentalId, start, nights));
    }

    [Fact]
    public async Task ExecuteAsync_ValidInput_ReturnsCalendarViewModel()
    {
        // Arrange
        var rentalId = 1;
        var start = new DateTime(2023, 02, 23);
        var nights = 3;
        var bookings = new List<BookingDto>
        {
            new() { Id = 1, RentalId = rentalId, Start = start.AddDays(1), Nights = 2 },
            new() { Id = 2, RentalId = rentalId, Start = start.AddDays(2), Nights = 1, PreparationTimeInDays = 1 },
        };
        _rentalRepositoryMock.Setup(x => x.IsExists(rentalId)).ReturnsAsync(true);
        _bookingRepositoryMock.Setup(x => x.GetAll(rentalId, start, start.AddDays(nights))).ReturnsAsync(bookings);

        // Act
        var result = await _underTest.ExecuteAsync(rentalId, start, nights);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rentalId, result.RentalId);
        Assert.Equal(nights, result.Dates.Count);
    }
}
