using Models.DataModels;
using Repository.Repository;

namespace VacationRental.Api.Operations.UnitOperations;

public sealed class UnitUpdateOperation : IUnitUpdateOperation
{
    private readonly IUnitRepository _unitRepository;

    public UnitUpdateOperation(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public Task ExecuteAsync(RentalDto rental, IEnumerable<int> newUnits)
    {
        return DoExecuteAsync(rental, newUnits);
    }

    /// <summary>
    /// Updates units for rental
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private async Task DoExecuteAsync(RentalDto rental, IEnumerable<int> newUnits)
    {
        if (rental.Units.Count() < newUnits.Count())
        {
            var listOfAllUnits = await _unitRepository.GetAll();
            var latestIdUnitValue = listOfAllUnits.Count() + 1;

            var unitsToAdd = newUnits.Except(rental.Units);

            for (var i = 0; i < unitsToAdd.Count(); i++)
            {
                var unitDto = new UnitDto
                {
                    Id = latestIdUnitValue + i,
                    RentalId = rental.Id,
                };

                await _unitRepository.Create(unitDto.Id, unitDto);
            }
        }

        if (rental.Units.Count() >= newUnits.Count())
        {
            var unitsToDelete = rental.Units.Except(newUnits);

            foreach (var unitId in unitsToDelete)
            {
                await _unitRepository.Delete(unitId);
            }
        }
    }
}
