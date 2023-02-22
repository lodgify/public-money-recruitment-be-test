using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Models.ViewModels.Rental;
using Moq;
using Repository.Repository;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.BookingOperations;
using VacationRental.Api.Operations.RentalsOperations;
using VacationRental.Api.Operations.UnitOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.RentalOperationsTests;

public sealed class RentalUpdateOperationTests
{
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IBookingPreparationCheckOperation> _bookingPreparationCheckOperationMock;
    private readonly Mock<IUnitUpdateOperation> _unitUpdateOperationMock;

    private readonly IRentalUpdateOperation _underTest;

    private readonly int _rentalId = 1;


    public RentalUpdateOperationTests()
    {
        _rentalRepositoryMock = new Mock<IRentalRepository>();
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _bookingPreparationCheckOperationMock = new Mock<IBookingPreparationCheckOperation>();
        _unitUpdateOperationMock = new Mock<IUnitUpdateOperation>();

        _underTest = new RentalUpdateOperation(
            _rentalRepositoryMock.Object,
            _bookingRepositoryMock.Object,
            _bookingPreparationCheckOperationMock.Object,
            _unitUpdateOperationMock.Object
        );
    }

    [Fact]
    public async Task ThrowsRentalIdValidationExceptionWhenRentalIdZero()
    {
        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(0, new UpdateRentalViewModel()));
    }


    [Fact]
    public async Task ThrowsNotFoundExceptionWhenRentalNotFound()
    {
        // Arrange
        _rentalRepositoryMock.Setup(x => x.IsExists(_rentalId)).ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _underTest.ExecuteAsync(_rentalId, new UpdateRentalViewModel()));
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenNoChangesMade()
    {
        // Arrange
        var existingRental = new RentalDto { Units = new List<int> { 1, 2, 3 }, PreparationTimeInDays = 2 };

        _rentalRepositoryMock.Setup(x => x.IsExists(_rentalId)).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(_rentalId)).ReturnsAsync(existingRental);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(_rentalId,
            new UpdateRentalViewModel { Units = 3, PreparationTimeInDays = 2 }));
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenUnitsAndPreparationTimeAreTheSame()
    {
        // Arrange
        var model = new UpdateRentalViewModel { Units = 2, PreparationTimeInDays = 3 };
        var oldRental = new RentalDto { Units = new List<int> { 1, 2 }, PreparationTimeInDays = 3 };

        _rentalRepositoryMock.Setup(x => x.IsExists(_rentalId)).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(_rentalId)).ReturnsAsync(oldRental);

        // Act + Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(_rentalId, model));

        _rentalRepositoryMock.Verify(x => x.IsExists(_rentalId), Times.Once);
        _rentalRepositoryMock.Verify(x => x.Get(_rentalId), Times.Once);
    }

    [Fact]
    public async Task SuccsessIncreaseUnitsWhenDataValid()
    {
        // Arrange
        var model = new UpdateRentalViewModel { Units = 3, PreparationTimeInDays = 3 };
        var oldRental = new RentalDto { Units = new List<int> { 1, 2 }, PreparationTimeInDays = 3 };
        var updatedRental = new RentalDto { Units = new List<int> { 1, 2, 3 }, PreparationTimeInDays = 3 };

        _rentalRepositoryMock.Setup(x => x.IsExists(_rentalId)).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(_rentalId)).ReturnsAsync(oldRental);
        _rentalRepositoryMock.Setup(x => x.Update(_rentalId, It.IsAny<RentalDto>())).ReturnsAsync(updatedRental);
        _unitUpdateOperationMock.Setup(x => x.ExecuteAsync(oldRental, It.IsAny<List<int>>()));

        // Act + Assert
        var result = await _underTest.ExecuteAsync(_rentalId, model);

        Assert.Equal(result.PreparationTimeInDays, model.PreparationTimeInDays);
        Assert.Equal(result.Units, model.Units);
        _rentalRepositoryMock.Verify(x => x.IsExists(_rentalId), Times.Once);
        _rentalRepositoryMock.Verify(x => x.Get(_rentalId), Times.Once);
    }

    [Fact]
    public async Task SuccsessDecreaseUnitsWhenDataValid()
    {
        // Arrange
        var model = new UpdateRentalViewModel { Units = 2, PreparationTimeInDays = 3 };
        var oldRental = new RentalDto { Units = new List<int> { 1, 2, 3 }, PreparationTimeInDays = 2 };
        var updatedRental = new RentalDto { Units = new List<int> { 1, 2 }, PreparationTimeInDays = 3 };

        _rentalRepositoryMock.Setup(x => x.IsExists(_rentalId)).ReturnsAsync(true);
        _rentalRepositoryMock.Setup(x => x.Get(_rentalId)).ReturnsAsync(oldRental);
        _rentalRepositoryMock.Setup(x => x.Update(_rentalId, It.IsAny<RentalDto>())).ReturnsAsync(updatedRental);
        _unitUpdateOperationMock.Setup(x => x.ExecuteAsync(oldRental, It.IsAny<List<int>>()));

        // Act + Assert
        var result = await _underTest.ExecuteAsync(_rentalId, model);

        Assert.Equal(result.PreparationTimeInDays, model.PreparationTimeInDays);
        Assert.Equal(result.Units, model.Units);
        _rentalRepositoryMock.Verify(x => x.IsExists(_rentalId), Times.Once);
        _rentalRepositoryMock.Verify(x => x.Get(_rentalId), Times.Once);
    }
}
