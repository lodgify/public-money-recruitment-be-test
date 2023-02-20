using Models.ViewModels.Rental;

namespace VacationRental.Api.Operations.RentalsOperations;

public interface IRentalUpdateOperation
{
    Task<UpdateRentalViewModel> ExecuteAsync(int rentalId, UpdateRentalViewModel model);
}

