using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;
using VacationRental.Api.Constants;
using VacationRental.Api.Operations.RentalsOperations;

namespace VacationRental.Api.Controllers;

[Route(RouteConstants.DefaultRoute)]
[ApiController]
public class RentalController : ControllerBase
{
    private readonly IRentalGetOperation _rentalGetOperation;
    private readonly IRentalCreateOperation _rentalCreateOperation;

    public RentalController(IRentalGetOperation rentalGetOperation, IRentalCreateOperation rentalCreateOperation)
    {
        _rentalGetOperation = rentalGetOperation;
        _rentalCreateOperation = rentalCreateOperation;
    }

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

    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
    {
        var result = await _rentalCreateOperation.ExecuteAsync(model);

        return result;
    }
}
