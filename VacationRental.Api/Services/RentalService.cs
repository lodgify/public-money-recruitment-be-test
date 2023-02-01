using System;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Providers;
using VacationRental.Api.Storage;

namespace VacationRental.Api.Services;

public sealed class RentalService : IRentalService
{
    private readonly IStateManager _state;
    private readonly IIdGenerator _idGenerator;
    
    public RentalService(
        IIdGenerator idGenerator,
        IStateManager state)
    {
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public bool IsExists(int rentalId) => _state.Rentals.Any(x => x.Id == rentalId);

    public RentalViewModel Get(int rentalId)
    {
        try
        {
            return _state.Rentals.Single(x => x.Id == rentalId);
        }
        catch
        {
            throw new ArgumentException($"{nameof(rentalId)} doesn't exist.");
        }
    }

    public ResourceIdViewModel Add(RentalViewModel viewModel)
    {
        var newId = _idGenerator.Generate(_state.Rentals);
        viewModel.Id = newId;
        _state.Rentals.Add(viewModel);
        return new ResourceIdViewModel
        {
            Id = viewModel.Id
        };
    }
}