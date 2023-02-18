using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;
using VacationRental.Api.Constants;
using VacationRental.Api.Operations.BookingOperations;

namespace VacationRental.Api.Controllers;

[Route(RouteConstants.DefaultRoute)]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingCreateOperation _bookingCreateOperation;
    private readonly IBookingGetOperation _bookingGetOperation;

    public BookingController(
        IBookingCreateOperation bookingCreateOperation,
        IBookingGetOperation bookingGetOperation)
    {
        _bookingCreateOperation = bookingCreateOperation;
        _bookingGetOperation = bookingGetOperation;
    }

    /// <summary>
    /// Retrieves a specific booking by unique id.
    /// </summary>
    [HttpGet]
    [Route("{bookingId:int}")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status404NotFound)]
    public async Task<BookingViewModel> Get(int bookingId)
    {
        var result = await _bookingGetOperation.ExecuteAsync(bookingId);

        return result;
    }

    /// <summary>
    /// Creates new booking.
    /// </summary>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    public async Task<ResourceIdViewModel> Post(BookingBindingViewModel model)
    {
        var result = await _bookingCreateOperation.ExecuteAsync(model);

        return result;
    }
}
