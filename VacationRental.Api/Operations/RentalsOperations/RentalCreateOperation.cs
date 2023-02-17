using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations
{
    public sealed class RentalCreateOperation : IRentalCreateOperation
    {
        public RentalCreateOperation()
        {
        }

        public ResourceIdViewModel ExecuteAsync(RentalBindingModel model)
        {
            return DoExecute(model);
        }

        private ResourceIdViewModel DoExecute(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units
            });

            return key;
        }
    }
}
