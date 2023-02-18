using Models.ViewModels;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.RentalsOperations;

public sealed class RentalGetOperation : IRentalGetOperation
{
    private readonly IRentalRepository _rentalRepository;

    public RentalGetOperation(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public RentalViewModel ExecuteAsync(int rentalId)
    {
        if (rentalId <= 0)
            throw new ApplicationException("Wrong Id");

        return DoExecute(rentalId);
    }

    private RentalViewModel DoExecute(int rentalId)
    {
        if (!_rentalRepository.IsExists(rentalId))
            throw new ApplicationException("Rental not found");

        return _rentalRepository.Get(rentalId);
    }
}

