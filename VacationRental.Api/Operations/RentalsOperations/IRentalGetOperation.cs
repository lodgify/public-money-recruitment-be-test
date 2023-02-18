using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations;

public interface IRentalGetOperation
{
    RentalViewModel ExecuteAsync(int rentalId);
}

