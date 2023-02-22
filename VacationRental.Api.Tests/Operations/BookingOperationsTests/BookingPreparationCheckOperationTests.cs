using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Moq;
using Repository.Repository;
using VacationRental.Api.Operations.BookingOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.BookingOperationsTests;

public class BookingPreparationCheckOperationTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;

    private readonly IBookingPreparationCheckOperation _underTests;

    public BookingPreparationCheckOperationTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();

        _underTests = new BookingPreparationCheckOperation(_bookingRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenInvalidInput()
    {
        // Arrange
        var rentalId = 1;
        var preparationTimeInDays = 2;
        var orderStartDate = new DateTime(2023, 3, 1);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _underTests.ExecuteAsync(0, preparationTimeInDays, orderStartDate));

        await Assert.ThrowsAsync<ValidationException>(() =>
            _underTests.ExecuteAsync(rentalId, 0, orderStartDate));
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenPreparationTimeTooBig()
    {
        // Arrange
        var rentalId = 1;
        var preparationTimeInDays = 2;
        var orderStartDate = new DateTime(2023, 3, 1);

        var bookings = new List<BookingDto>
        {
            new()
            {
                Id = 1, RentalId = rentalId, PreparationTimeInDays = 1, Start = new DateTime(2023, 3, 2), Nights = 2,
                Unit = 1
            },
            new()
            {
                Id = 2, RentalId = rentalId, PreparationTimeInDays = 1, Start = new DateTime(2023, 3, 4), Nights = 2,
                Unit = 1
            },
            new()
            {
                Id = 3, RentalId = rentalId, PreparationTimeInDays = 2, Start = new DateTime(2023, 3, 8), Nights = 2,
                Unit = 2
            }
        };

        _bookingRepositoryMock.Setup(x => x.GetAll(rentalId, orderStartDate)).ReturnsAsync(bookings);

        // Act
        // Assert
        await Assert.ThrowsAsync<ValidationException>(() =>  _underTests.ExecuteAsync(rentalId, preparationTimeInDays, orderStartDate));
    }


    [Fact]
    public async Task ShouldReturnExpextedWhenDataValid()
    {
        // Arrange
        var rentalId = 1;
        var preparationTimeInDays = 1;
        var orderStartDate = new DateTime(2022, 2, 2);

        var bookings = new List<BookingDto>
        {
            new()
            {
                Id = 1, 
                RentalId = rentalId, 
                PreparationTimeInDays = 2, 
                Start = new DateTime(2022, 2, 2),
                Nights = 2,
                Unit = 1
            },
            new()
            {
                Id = 2,
                RentalId = rentalId,
                PreparationTimeInDays = 2, 
                Start = new DateTime(2022, 2, 4), 
                Nights = 2,
                Unit = 1
            },
            new()
            {
                Id = 3, 
                RentalId = rentalId, 
                PreparationTimeInDays = 2, 
                Start = new DateTime(2022, 2, 8), 
                Nights = 2,
                Unit = 2
            }
        };

        _bookingRepositoryMock.Setup(x => x.GetAll(rentalId, orderStartDate)).ReturnsAsync(bookings);

        var result = await _underTests.ExecuteAsync(rentalId, preparationTimeInDays, orderStartDate);
        // Act
        // Assert
        Assert.Equal(result.Count(), bookings.Count());
    }
}
