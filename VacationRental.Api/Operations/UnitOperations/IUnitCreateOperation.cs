using Models.ViewModels.Rental;

namespace VacationRental.Api.Operations.UnitOperations;

public interface IUnitCreateOperation
{
    Task<IEnumerable<int>> ExecuteAsync(RentalViewModel model);
}
