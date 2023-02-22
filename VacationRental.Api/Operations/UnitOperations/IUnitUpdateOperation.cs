using Models.DataModels;

namespace VacationRental.Api.Operations.UnitOperations;

public interface IUnitUpdateOperation
{
    Task ExecuteAsync(RentalDto rental, IEnumerable<int> newUnits);
}
