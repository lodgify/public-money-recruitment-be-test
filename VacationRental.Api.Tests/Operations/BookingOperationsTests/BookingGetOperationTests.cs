using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Models.ViewModels.Booking;
using Moq;
using Repository.Repository;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.BookingOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.BookingOperationsTests;

public class BookingGetOperationTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly BookingGetOperation _underTest;

    public BookingGetOperationTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();

        _underTest = new BookingGetOperation(_bookingRepositoryMock.Object);
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenInvalidBookingId()
    {
        // Arrange
        int bookingId = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(bookingId));
    }

    [Fact]
    public async Task ThrowsNotFoundExceptionWhenNonExistingBookingId()
    {
        // Arrange
        int bookingId = 1;
        _bookingRepositoryMock.Setup(repo => repo.IsExists(bookingId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _underTest.ExecuteAsync(bookingId));
    }

    [Fact]
    public async Task ReturnsBookingViewModelWhenValidBookingId()
    {
        // Arrange
        int bookingId = 1;
        var bookingDto = new BookingDto { Id = bookingId, RentalId = 1, Nights = 1, Unit = 1 };
        var expectedViewModel = new BookingViewModel { Id = bookingId, RentalId = 1, Nights = 1 };
        _bookingRepositoryMock.Setup(repo => repo.IsExists(bookingId)).ReturnsAsync(true);
        _bookingRepositoryMock.Setup(repo => repo.Get(bookingId)).ReturnsAsync(bookingDto);

        // Act
        var result = await _underTest.ExecuteAsync(bookingId);

        // Assert
        Assert.Equal(expectedViewModel.Id, result.Id);
        Assert.Equal(expectedViewModel.RentalId, result.RentalId);
        Assert.Equal(expectedViewModel.Nights, result.Nights);
    }
}
