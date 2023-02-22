using Mapster;
using Models.DataModels;
using Models.ViewModels.Rental;
using Moq;
using Repository.Repository;
using VacationRental.Api.Operations.RentalsOperations;
using VacationRental.Api.Operations.UnitOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.RentalOperationsTests;

public class RentalCreateOperationTests
{
    private readonly Mock<IRentalRepository> _mockRentalRepository;
    private readonly Mock<IUnitCreateOperation> _mockUnitCreateOperation;

    public RentalCreateOperationTests()
    {
        _mockRentalRepository = new Mock<IRentalRepository>();
        _mockUnitCreateOperation = new Mock<IUnitCreateOperation>();
    }

    [Fact]
    public async Task ExecuteAsync_CreatesNewRentalAndRelatedUnits()
    {
        // Arrange
        var rentalBindingModel = new RentalBindingModel { Units = 1, PreparationTimeInDays = 3 };
        var rentalDto = rentalBindingModel.Adapt<RentalDto>();
        var expectedRentalId = 1;
        var expectedResourceIdViewModel = new ResourceIdViewModel { Id = expectedRentalId };

        _mockRentalRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<RentalDto>());
        _mockUnitCreateOperation.Setup(op => op.ExecuteAsync(It.IsAny<RentalViewModel>())).ReturnsAsync(new List<int> { 1 });
        _mockRentalRepository.Setup(repo => repo.Create(expectedRentalId, It.IsAny<RentalDto>())).ReturnsAsync(rentalDto);

        var underTest = new RentalCreateOperation(_mockRentalRepository.Object, _mockUnitCreateOperation.Object);

        // Act
        var result = await underTest.ExecuteAsync(rentalBindingModel);

        // Assert
        Assert.Equal(expectedResourceIdViewModel.Id, result.Id);

        _mockUnitCreateOperation.Verify(op => op.ExecuteAsync(It.IsAny<RentalViewModel>()), Times.Once());
        _mockRentalRepository.Verify(repo => repo.Create(expectedRentalId, It.IsAny<RentalDto>()), Times.Once());
    }
}
