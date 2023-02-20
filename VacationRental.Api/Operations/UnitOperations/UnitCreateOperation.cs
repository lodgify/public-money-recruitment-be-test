using Models.DataModels;
using Models.ViewModels.Rental;
using Repository.Repository;

namespace VacationRental.Api.Operations.UnitOperations;

public sealed class UnitCreateOperation : IUnitCreateOperation
{
    private readonly IUnitRepository _unitRepository;

    public UnitCreateOperation(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public Task<IEnumerable<int>> ExecuteAsync(RentalViewModel model)
    {
        return DoExecuteAsync(model);
    }

    /// <summary>
    /// Creates new Units for specific rental.
    /// Task.WhenAll is better to use, but dictionary restrictions and too fast execution fake asynchrony can make collisions
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private async Task<IEnumerable<int>> DoExecuteAsync(RentalViewModel model)
    {
        var listOfAllUnits = await _unitRepository.GetAll();
        var latestIdUnitValue = listOfAllUnits.Count() + 1;
        var units = new List<UnitDto>();

        for (var i = 0; i < model.Units; i++)
        {
            var unitDto = new UnitDto
            {
                Id = latestIdUnitValue + i,
                RentalId = model.Id,
            };

            var unit = await _unitRepository.Create(unitDto.Id, unitDto);
            units.Add(unit);
        }

        return units.Select(_ => _.Id);
    }
}
