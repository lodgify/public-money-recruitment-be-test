using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Rental;
using VacationRental.Api.Constants;
using VacationRental.Api.Operations.RentalsOperations;

namespace VacationRental.Api.Controllers;

[Route(RouteConstants.DefaultRoute)]
[ApiController]
public sealed class RentalController : ControllerBase
{
    private readonly IRentalGetOperation _rentalGetOperation;
    private readonly IRentalCreateOperation _rentalCreateOperation;
    private readonly IRentalUpdateOperation _rentalUpdateOperation;

    public RentalController(
        IRentalGetOperation rentalGetOperation, 
        IRentalCreateOperation rentalCreateOperation, 
        IRentalUpdateOperation rentalUpdateOperation)
    {
        _rentalGetOperation = rentalGetOperation;
        _rentalCreateOperation = rentalCreateOperation;
        _rentalUpdateOperation = rentalUpdateOperation;
    }

    /// <summary>
    /// Retrieves a specific rental by unique id.
    /// </summary>
    [HttpGet]
    [Route("{rentalId:int}")]
    [ProducesResponseType(typeof(RentalViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status404NotFound)]
    public async Task<RentalViewModel> Get(int rentalId)
    {
        var result = await _rentalGetOperation.ExecuteAsync(rentalId);

        return result;
    }

    /// <summary>
    /// Creates rental.
    /// </summary>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
    {
        var result = await _rentalCreateOperation.ExecuteAsync(model);

        return result;
    }


    /// <summary>
    /// Updates a specific rental by unique id.
    /// </summary>
    [HttpPut]
    [Route("{rentalId:int}")]
    [ProducesResponseType(typeof(UpdateRentalViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status404NotFound)]
    public async Task<UpdateRentalViewModel> Put(int rentalId, UpdateRentalViewModel model)
    {
        var result = await _rentalUpdateOperation.ExecuteAsync(rentalId, model);

        return result;
    }
}
