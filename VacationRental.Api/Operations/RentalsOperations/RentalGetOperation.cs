using System.ComponentModel.DataAnnotations;
using Mapster;
using Models.DataModels;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Rental;

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

        var result = await _rentalRepository.Get(rentalId);

        return result.Adapt<RentalViewModel>(RentalUnitsAdapterConfig());
    }

    private TypeAdapterConfig RentalUnitsAdapterConfig() => TypeAdapterConfig<RentalDto, RentalViewModel>
           .NewConfig()
           .Map(dest => dest.Units, src => src.Units.Count)
           .Config;

}
