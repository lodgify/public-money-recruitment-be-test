using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations;

public interface IRentalCreateOperation
{
    Task<ResourceIdViewModel> ExecuteAsync(RentalBindingModel model);
}
