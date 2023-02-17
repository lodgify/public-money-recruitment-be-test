using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations
{
    public interface IRentalCreateOperation
    {
        ResourceIdViewModel ExecuteAsync(RentalBindingModel model);
    }
}
