using System.ComponentModel.DataAnnotations;
using Mapster;
using Models.DataModels;
using Repository.Repository;
using VacationRental.Api.Constants;
using VacationRental.Api.Exceptions;
using Models.ViewModels.Rental;
using VacationRental.Api.Operations.BookingOperations;
using VacationRental.Api.Operations.UnitOperations;

namespace VacationRental.Api.Operations.RentalsOperations;

public sealed class RentalUpdateOperation : IRentalUpdateOperation
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitUpdateOperation _unitUpdateOperation;
    private readonly IBookingPreparationCheckOperation _bookingPreparationCheckOperation;

    public RentalUpdateOperation(
        IRentalRepository rentalRepository, 
        IBookingRepository bookingRepository,
        IBookingPreparationCheckOperation bookingPreparationCheckOperation,
        IUnitUpdateOperation unitUpdateOperation)
    {
        _rentalRepository = rentalRepository;
        _bookingRepository = bookingRepository;
        _unitUpdateOperation = unitUpdateOperation;
        _bookingPreparationCheckOperation = bookingPreparationCheckOperation;
    }

    public Task<UpdateRentalViewModel> ExecuteAsync(int rentalId, UpdateRentalViewModel model)
    {
        if (rentalId <= 0)
            throw new ValidationException(ExceptionMessageConstants.RentalIdValidationError);

        return DoExecuteAsync(rentalId, model);
    }

    /// <summary>
    /// Checks if it possible to update rental unis and preparation time for existing bookings.
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

        if (oldRental.Units?.Count == model.Units && oldRental.PreparationTimeInDays == model.PreparationTimeInDays)
            throw new ValidationException(ExceptionMessageConstants.ValidationUpdateError);

        var bookings = await _bookingPreparationCheckOperation.ExecuteAsync(rentalId, model.PreparationTimeInDays, DateTime.Today);

        var updateRentalModel = model.Adapt<RentalDto>();

        updateRentalModel.Units = oldRental.Units;
        updateRentalModel.Id = rentalId;
        if (oldRental.Units?.Count != model.Units)
        {
            var newUnitsList = GetNewUnitsList(bookings, oldRental.Units, model.Units);
            await _unitUpdateOperation.ExecuteAsync(updateRentalModel, newUnitsList);

            updateRentalModel.Units = newUnitsList;
        }

        foreach (var booking in bookings)
        {
            booking.PreparationTimeInDays = model.PreparationTimeInDays;
            await _bookingRepository.Update(booking.Id, booking);
        }

        var result = await  _rentalRepository.Update(rentalId, updateRentalModel);
        
        return result.Adapt<UpdateRentalViewModel>(RentalUnitsAdapterConfig());
    }

    private List<int> GetNewUnitsList(IEnumerable<BookingDto> bookings, IList<int> oldRentalUnitsIds, int newUnitsCount)
    {
        var newRentalUnitsList = new List<int>();
        if (oldRentalUnitsIds.Count < newUnitsCount)
        {
            var newAdditionalUnitsCount = newUnitsCount - oldRentalUnitsIds.Count;
            newRentalUnitsList.AddRange(oldRentalUnitsIds);
            
            for (var i = 0; i < newAdditionalUnitsCount; i ++)
                newRentalUnitsList.Add(-1);
        }

        if (oldRentalUnitsIds.Count > newUnitsCount)
        {
            var unitsToDeleteCount = oldRentalUnitsIds.Count - newUnitsCount;
            var allBookingsList = bookings
                        .Select(_ => _.Unit)
                        .Distinct()
                        .ToList();

            var avaliableToDeleteUnitsCount = oldRentalUnitsIds.Count - allBookingsList.Count;
            if (avaliableToDeleteUnitsCount < unitsToDeleteCount)
                throw new ValidationException(ExceptionMessageConstants.CreateUnitsValidationError);

            var avaliableToDeleteUnits = oldRentalUnitsIds.Except(allBookingsList).ToList();

            newRentalUnitsList.AddRange(allBookingsList);
            for (var i = 0; i < avaliableToDeleteUnitsCount - unitsToDeleteCount; i++)
            {
                newRentalUnitsList.Add(avaliableToDeleteUnits[i]);
            }
        }

        return newRentalUnitsList;
    }

    private TypeAdapterConfig RentalUnitsAdapterConfig() => TypeAdapterConfig<RentalDto, UpdateRentalViewModel>
            .NewConfig()
            .Map(dest => dest.Units, src => src.Units.Count)
            .Config;

}

