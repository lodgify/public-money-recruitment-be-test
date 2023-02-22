using Models.DataModels;
using Models.ViewModels.Rental;
using Moq;
using Repository.Repository;
using VacationRental.Api.Operations.UnitOperations;
using Xunit;

namespace VacationRental.Api.Tests.Operations.UnitOperationsTests;

public class UnitCreateOperationTests
{
    private readonly Mock<IUnitRepository> _unitRepositoryMock;
    private readonly RentalViewModel _model;

    private readonly UnitCreateOperation _underTest;

    public UnitCreateOperationTests()
    {
        _unitRepositoryMock = new Mock<IUnitRepository>();
        _model = new RentalViewModel { Id = 1, Units = 3 };

        _underTest = new UnitCreateOperation(_unitRepositoryMock.Object);
    }

    [Fact]
    public async Task ReturnsCreatedUnitIdsWhenValidModel()
    {
        // Arrange
        var createdUnits = new List<UnitDto>
        {
            new() { Id = 1, RentalId = _model.Id },
            new() { Id = 2, RentalId = _model.Id },
            new() { Id = 3, RentalId = _model.Id }
        };
        _unitRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<UnitDto>());

        for (var i = 0; i < _model.Units; i++)
        {
            var unitDto = new UnitDto
            {
                Id = i + 1,
                RentalId = _model.Id
            };
            _unitRepositoryMock.Setup(repo => repo.Create(unitDto.Id, unitDto)).ReturnsAsync(createdUnits[i]);
        }

        // Act
        var result = await _underTest.ExecuteAsync(_model);

        // Assert
        _unitRepositoryMock.Verify(repo => repo.Create(It.IsAny<int>(), It.IsAny<UnitDto>()), Times.Exactly(_model.Units));
    }

    [Fact]
    public async Task CreatesUnitsWhenCorrectRentalId()
    {
        // Arrange
        _unitRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<UnitDto>());

        // Act
        await _underTest.ExecuteAsync(_model);

        // Assert
        _unitRepositoryMock.Verify(repo => repo.Create(It.IsAny<int>(), It.Is<UnitDto>(u => u.RentalId == _model.Id)), Times.Exactly(_model.Units));
    }
}
