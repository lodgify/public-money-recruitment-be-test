using Models.DataModels;
using Moq;
using Repository.Repository;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Operations.UnitOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.UnitOperationsTests;

public class UnitListGetOperationTests
{
    private readonly Mock<IUnitRepository> _unitRepositoryMock;
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;
    private readonly UnitListGetOperation _underTest;

    public UnitListGetOperationTests()
    {
        _unitRepositoryMock = new Mock<IUnitRepository>();
        _rentalRepositoryMock = new Mock<IRentalRepository>();

        _underTest = new UnitListGetOperation(_unitRepositoryMock.Object, _rentalRepositoryMock.Object);
    }


    [Fact]
    public async Task ThrowsNotFoundExceptionWhenInvalidUnitId()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _underTest.ExecuteAsync(-1));
    }

    [Fact]
    public async Task ThrowsNotFoundExceptionWhenRentalNotFound()
    {
        // Arrange
        var rentalId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _underTest.ExecuteAsync(rentalId));
    }

    [Fact]
    public async Task ExecuteAsync_ValidUnitId_ReturnsUnitIds()
    {
        // Arrange
        var rentalId = 1;
        _unitRepositoryMock.Setup(repo => repo.GetAll(rentalId))
            .ReturnsAsync(new List<UnitDto> { new UnitDto { Id = 1 }, new UnitDto { Id = 2 } });
        _rentalRepositoryMock.Setup(repo => repo.IsExists(rentalId))
            .ReturnsAsync(true);

        // Act
        var result = await _underTest.ExecuteAsync(rentalId);

        // Assert
        Assert.Collection(result,
            id => Assert.Equal(1, id),
            id => Assert.Equal(2, id));
    }
}
