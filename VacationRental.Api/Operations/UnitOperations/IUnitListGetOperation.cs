namespace VacationRental.Api.Operations.UnitOperations;

public interface IUnitListGetOperation
{
    Task<IEnumerable<int>> ExecuteAsync(int rentalId);
}

