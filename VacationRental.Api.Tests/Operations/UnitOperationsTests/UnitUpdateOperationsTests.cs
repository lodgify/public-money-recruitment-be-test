using Models.DataModels;
using Moq;
using Repository.Repository;
using VacationRental.Api.Operations.UnitOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.UnitOperationsTests;

public class UnitUpdateOperationTests
{
    private readonly Mock<IUnitRepository> _mockUnitRepository;
    private readonly UnitUpdateOperation _unitUpdateOperation;

    public UnitUpdateOperationTests()
    {
        _mockUnitRepository = new Mock<IUnitRepository>();
        _unitUpdateOperation = new UnitUpdateOperation(_mockUnitRepository.Object);
    }

    [Fact]
    public async Task CreatesNewUnitsInRepositoryWhenDataValid()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = 1, Units = new List<int> { 1, 2, 3 } };
        var newUnits = new List<int> { 1, 2, 3, 4, 5 };

        _mockUnitRepository.Setup(repo => repo.GetAll())
                           .ReturnsAsync(new List<UnitDto>());

        // Act
        await _unitUpdateOperation.ExecuteAsync(rentalDto, newUnits);

        // Assert
        _mockUnitRepository.Verify(repo => repo.Create(It.IsAny<int>(), It.IsAny<UnitDto>()), Times.Exactly(newUnits.Count - rentalDto.Units.Count));
    }

    [Fact]
    public async Task DeletesUnitsFromRepositoryWhenDataValid()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = 1, Units = new List<int> { 1, 2, 3 } };
        var newUnits = new List<int> { 1, 3 };

        // Act
        await _unitUpdateOperation.ExecuteAsync(rentalDto, newUnits);

        // Assert
        _mockUnitRepository.Verify(repo => repo.Delete(2), Times.Once);
    }
}
