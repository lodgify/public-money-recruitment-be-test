using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Middleware.ExceptionHandling;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto;

namespace VacationRental.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CalendarController(ICalendarService calendarService, IMapper mapper, ILogger<CalendarController> logger)
    {
        _calendarService = calendarService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Searches for a rental item
    /// </summary>
    /// <param name="model">FilterBindingModel</param>
    /// <returns>RentalViewModel item</returns>
    /// <response code="200">Returns a CalendarViewModel item</response>
    /// <response code="400">Returns a validation error message</response>
    [HttpGet]
    [HandleExceptions]
    public async Task<IActionResult> Get([FromQuery]CalendarFilterModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(model)}");
            return BadRequest(ModelState);
        }

        var result = _calendarService.Get(_mapper.Map<CalendarFilterModel, CalendarFilterDto>(model));

        return Ok(result);
    }
}