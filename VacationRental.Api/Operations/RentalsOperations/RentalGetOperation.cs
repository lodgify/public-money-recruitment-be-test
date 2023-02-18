using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Operations.RentalsOperations;

public sealed class RentalGetOperation : IRentalGetOperation
{
    private readonly IRentalRepository _rentalRepository;

    public RentalGetOperation(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public Task<RentalViewModel> ExecuteAsync(int rentalId)
    {
        if (rentalId <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalIdValidationError);

        return DoExecuteAsync(rentalId);
    }

    private async Task<RentalViewModel> DoExecuteAsync(int rentalId)
    {
        if (!await _rentalRepository.IsExists(rentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        return await Task.Run(() => _rentalRepository.Get(rentalId));
    }
}

