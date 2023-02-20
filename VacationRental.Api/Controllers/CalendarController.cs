using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Calendar;
using VacationRental.Api.Constants;
using VacationRental.Api.Operations.CalendarOperations;

namespace VacationRental.Api.Controllers;

[Route(RouteConstants.DefaultRoute)]
[ApiController]
public sealed class CalendarController : ControllerBase
{
    private readonly ICalendarGetOperation _calendarGetOperation;

    public CalendarController(ICalendarGetOperation calendarGetOperation)
    {
        _calendarGetOperation = calendarGetOperation;
    }

    /// <summary>
    /// Retrieves a rental booking status.
    /// </summary>
    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(CalendarViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SwaggerErrorMessageModel), StatusCodes.Status404NotFound)]
    public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
    {
        var result = await _calendarGetOperation.ExecuteAsync(rentalId, start, nights);

        return result;
    }
}