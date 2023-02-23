using System.ComponentModel.DataAnnotations;
using Mapster;
using Models.DataModels;
using Models.ViewModels.Rental;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Operations.UnitOperations;

namespace VacationRental.Api.Operations.RentalsOperations;

public sealed class RentalCreateOperation : IRentalCreateOperation
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUnitCreateOperation _unitCreateOperation;

    public RentalCreateOperation(IRentalRepository rentalRepository, IUnitCreateOperation unitCreateOperation)
    {
        _rentalRepository = rentalRepository;
        _unitCreateOperation = unitCreateOperation;
    }

    public Task<ResourceIdViewModel> ExecuteAsync(RentalBindingModel model)
    {
        if (model.Units <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalUnitValidationError);
        if (model.PreparationTimeInDays <= 0)
            throw new ValidationException(ExceptionMessageConstants.NightsValidationError);

        return DoExecuteAsync(model);
    }

    /// <summary>
    /// Creates new rental and list of related units
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private async Task<ResourceIdViewModel> DoExecuteAsync(RentalBindingModel model)
    {
        var rentals = await _rentalRepository.GetAll();
        var rentalId = rentals.Count() + 1;
        var rentalViewModel = model.Adapt<RentalViewModel>();
        var newRentalDto = model.Adapt<RentalDto>();

        newRentalDto.Id = rentalId;

        var unitsForRental = await _unitCreateOperation.ExecuteAsync(rentalViewModel);
        newRentalDto.Units = unitsForRental.ToList();

        await _rentalRepository.Create(rentalId, newRentalDto);

        var result = new ResourceIdViewModel { Id = rentalId };

        return result;
    }
}
