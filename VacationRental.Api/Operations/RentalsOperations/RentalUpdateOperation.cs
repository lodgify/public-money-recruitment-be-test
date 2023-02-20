using System.ComponentModel.DataAnnotations;
using Mapster;
using Models.DataModels;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Rental;
using VacationRental.Api.Operations.UnitOperations;

namespace VacationRental.Api.Operations.RentalsOperations;

public sealed class RentalUpdateOperation : IRentalUpdateOperation
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUnitCreateOperation _unitCreateOperation;

    public RentalUpdateOperation(IRentalRepository rentalRepository, IUnitCreateOperation unitCreateOperation)
    {
        _rentalRepository = rentalRepository;
        _unitCreateOperation = unitCreateOperation;
    }

    public Task<UpdateRentalViewModel> ExecuteAsync(int rentalId, UpdateRentalViewModel model)
    {
        if (rentalId <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalIdValidationError);

        return DoExecuteAsync(rentalId, model);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rentalId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValidationException"></exception>
    private async Task<UpdateRentalViewModel> DoExecuteAsync(int rentalId, UpdateRentalViewModel model)
    {
        if (!await _rentalRepository.IsExists(rentalId))
            throw new NotFoundException(ExceptionMessageConstants.RentalNotFound);

        var oldRental = await _rentalRepository.Get(rentalId);

        if (oldRental.Units?.Count > model.Units)
            throw new ValidationException(ExceptionMessageConstants.RentalUnitValidationError);

        var updateRentalModel = model.Adapt<RentalDto>();

        // TODO: logic if not possible to update rental PreparationTimeInDays throw badreq

        updateRentalModel.Units = oldRental.Units ?? new List<int>();
        if (oldRental.Units?.Count != model.Units)
            await UpdateRentalUnitsList(rentalId, model, updateRentalModel, oldRental);

        var result = await  _rentalRepository.Update(rentalId, updateRentalModel);
        
        return result.Adapt<UpdateRentalViewModel>(RentalUnitsAdapterConfig());
    }

    /// <summary>
    /// If the request is valid, changes the number of units (for now only increase)
    /// TODO : add logic for decrease 
    /// </summary>
    /// <param name="rentalId"></param>
    /// <param name="model"></param>
    /// <param name="updateRentalModel"></param>
    /// <param name="oldRental"></param>
    /// <returns></returns>
    private async Task UpdateRentalUnitsList(
        int rentalId, 
        UpdateRentalViewModel model, 
        RentalDto updateRentalModel,
        RentalDto oldRental)
    {
        var newUnitsCount = model.Units - oldRental.Units.Count;
        var newRentalUnits = new RentalViewModel
        {
            Id = rentalId,
            Units = newUnitsCount,
        };

        var newUnitsList = await _unitCreateOperation.ExecuteAsync(newRentalUnits);
        updateRentalModel.Units?.AddRange(newUnitsList);
    }

    private TypeAdapterConfig RentalUnitsAdapterConfig() => TypeAdapterConfig<RentalDto, UpdateRentalViewModel>
            .NewConfig()
            .Map(dest => dest.Units, src => src.Units.Count)
            .Config;

}

