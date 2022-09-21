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
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public RentalsController(IRentalService rentalService, IMapper mapper, ILogger<RentalsController> logger)
    {
        _rentalService = rentalService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Searches for a rental item
    /// </summary>
    /// <param name="rentalId">Unique Id that represents a rental</param>
    /// <returns>RentalViewModel item</returns>
    /// <response code="200 ">Returns a RentalViewModel item</response>
    /// <response code="400">Returns a validation error message</response>
    /// <response code="404">If the item is not found</response>
    [HttpGet]
    [HandleExceptions]
    [Route("{rentalId:int}")]
    public async Task<IActionResult> Get(int rentalId, CancellationToken cancellationToken = default)
    {
        if (rentalId == 0)
        {
            ModelState.AddModelError("rentalId", "rentalId is required");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(rentalId)}");
            return BadRequest(ModelState);
        }

        var booking = _rentalService.Get(rentalId);

        if (booking is null)
            return NotFound();

        return Ok(_mapper.Map<RentalDto, RentalViewModel>(booking));
    }

    /// <summary>
    /// Creates a new rental
    /// </summary>
    /// <returns>Unique Id that represents a new rental</returns>
    /// <response code="201">Returns an item identifier</response>
    /// <response code="400">Returns a validation error message</response>
    [HttpPost]
    [HandleExceptions]
    public async Task<IActionResult> Post(RentalBindingModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(model)}");
            return BadRequest(ModelState);
        }

        var key = _rentalService.Create(_mapper.Map<RentalBindingModel, RentalDto>(model));

        return StatusCode(StatusCodes.Status201Created, key);
    }

    /// <summary>
    /// Updates a rental
    /// </summary>
    /// <returns>Unique Id that represents a new rental</returns>
    /// <response code="201">Returns an item identifier</response>
    /// <response code="400">Returns a validation error message</response>
    [HttpPut]
    [HandleExceptions]
    [Route("{rentalId:int}")]
    public async Task<IActionResult> Put(RentalBindingModel model, int rentalId, CancellationToken cancellationToken = default)
    {
        if (rentalId == 0)
        {
            ModelState.AddModelError("rentalId", "rentalId is required");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogInformation($"BadRequest at {Request.Path}. Request details: {JsonSerializer.Serialize(model)}");
            return BadRequest(ModelState);
        }
        var rental = _mapper.Map<RentalBindingModel, RentalDto>(model);
        rental.Id = rentalId;

        var key = _rentalService.Update(rental);

        return StatusCode(StatusCodes.Status201Created, key);
    }
}