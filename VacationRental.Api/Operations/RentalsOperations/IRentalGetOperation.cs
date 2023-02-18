using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations;

public interface IRentalGetOperation
{
    Task<RentalViewModel> ExecuteAsync(int rentalId);
}

