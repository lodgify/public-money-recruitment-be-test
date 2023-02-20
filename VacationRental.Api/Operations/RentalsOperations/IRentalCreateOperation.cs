using Models.ViewModels.Rental;

namespace VacationRental.Api.Operations.RentalsOperations;

public interface IRentalCreateOperation
{
    Task<ResourceIdViewModel> ExecuteAsync(RentalBindingModel model);
}
