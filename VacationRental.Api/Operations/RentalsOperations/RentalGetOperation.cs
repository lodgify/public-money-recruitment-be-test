using Models.ViewModels;

namespace VacationRental.Api.Operations.RentalsOperations
{
    public class RentalGetOperation : IRentalGetOperation
    {
        public RentalGetOperation()
        {
        }

        public RentalViewModel ExecuteAsync(int rentalId)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Wrong Id");

            return DoExecute(rentalId);
        }

        private RentalViewModel DoExecute(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }
    }
}
