using System.ComponentModel.DataAnnotations;
using Models.DataModels;
using Models.ViewModels.Rental;
using Moq;
using Repository.Repository;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.RentalsOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.RentalOperationsTests;

public sealed class RentalGetOperationTests
{
    private readonly Mock<IRentalRepository> _mockRentalRepository;

    private readonly int _rentalId = 1;

    public RentalGetOperationTests()
    {
        _mockRentalRepository = new Mock<IRentalRepository>();
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenInvalidRentalId()
    {
        // Arrange
        var rentalId = 0;
        var rentalGetOperation = new RentalGetOperation(_mockRentalRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => rentalGetOperation.ExecuteAsync(rentalId));
    }

    [Fact]
    public async Task ThrowsNotFoundExceptionWithNonExistentRental()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = _rentalId, Units = new List<int>() };
        _mockRentalRepository.Setup(repo => repo.IsExists(_rentalId)).ReturnsAsync(false);
        _mockRentalRepository.Setup(repo => repo.Get(_rentalId)).ReturnsAsync(rentalDto);
        var rentalGetOperation = new RentalGetOperation(_mockRentalRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => rentalGetOperation.ExecuteAsync(_rentalId));
    }

    [Fact]
    public async Task ReturnsRentalViewModelWhenValidRentalId()
    {
        // Arrange
        var rentalId = 1;
        var unitDto = new UnitDto { Id = 1, RentalId = _rentalId };
        var rentalDto = new RentalDto { Id = rentalId, Units = new List<int> { unitDto.Id } };
        var rentalViewModel = new RentalViewModel { Id = rentalId, Units = 1 };
        _mockRentalRepository.Setup(repo => repo.IsExists(rentalId)).ReturnsAsync(true);
        _mockRentalRepository.Setup(repo => repo.Get(rentalId)).ReturnsAsync(rentalDto);
        var rentalGetOperation = new RentalGetOperation(_mockRentalRepository.Object);

        // Act
        var result = await rentalGetOperation.ExecuteAsync(rentalId);

        // Assert
        Assert.Equal(rentalViewModel.Id, result.Id);
        Assert.Equal(rentalViewModel.Units, result.Units);
    }
}
