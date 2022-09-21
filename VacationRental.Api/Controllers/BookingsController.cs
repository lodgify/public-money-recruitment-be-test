using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Middleware.ExceptionHandling;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto;

namespace VacationRental.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public BookingsController(IBookingService bookingService, IMapper mapper, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Searches for a booking item
    /// </summary>
    /// <param name="bookingId">Unique Id that represents a booking</param>
    /// <returns>BookingViewModel item</returns>
    /// <response code="200">Returns a BookingViewModel item</response>
    /// <response code="400">Returns a validation error message</response>
    /// <response code="404">If the item is not found</response>
    [HttpGet]
    [Route("{bookingId:int}")]
    [HandleExceptions]
    public async Task<IActionResult> Get(int bookingId, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(bookingId)}");
            return BadRequest(ModelState);
        }

        var booking = _bookingService.Get(bookingId);

        if (booking is null)
            return NotFound();

        return Ok(_mapper.Map<BookingDto, BookingViewModel>(booking));
    }

    /// <summary>
    /// Creates a new booking
    /// </summary>
    /// <returns>Unique Id that represents a new booking</returns>
    /// <response code="201">Returns an item identifier</response>
    /// <response code="400">Returns a validation error message</response>
    [HttpPost]
    [HandleExceptions]
    public async Task<IActionResult> Post(BookingBindingModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(model)}");
            return BadRequest(ModelState);
        }

        var key = _bookingService.Create(_mapper.Map<BookingBindingModel, BookingDto>(model));

        return StatusCode(StatusCodes.Status201Created, key);
    }
}