using System.ComponentModel.DataAnnotations;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;

namespace VacationRental.Api.Operations.UnitOperations;

public sealed class UnitListGetOperation : IUnitListGetOperation
{
    private readonly IUnitRepository _unitRepository;
    private readonly IRentalRepository _rentalRepository;

    public UnitListGetOperation(IUnitRepository unitRepository, IRentalRepository rentalRepository)
    {
        _unitRepository = unitRepository;
        _rentalRepository = rentalRepository;
    }

    public Task<IEnumerable<int>> ExecuteAsync(int unitId)
    {
        if (unitId < 0)
            throw new ValidationException(ExceptionMessageConstants.UnitIdValidationError);

        return DoExecuteAsync(unitId);
    }

    private async Task<IEnumerable<int>> DoExecuteAsync(int rentalId)
    {
        if (!await _rentalRepository.IsExists(rentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var result = await _unitRepository.GetAll(rentalId);

        return result.Select(_ => _.Id);
    }
}

