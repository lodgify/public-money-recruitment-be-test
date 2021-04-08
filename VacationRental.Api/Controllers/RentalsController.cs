using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Services;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalsService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(
            IRentalService rentalsService,
            ILogger<RentalsController> logger)
        {
            _rentalsService = rentalsService;
            _logger = logger;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            try
            {
                if (!await _rentalsService.RentalExists(rentalId))
                    return NotFound("Rental not found");

                var rental = await _rentalsService.GetRentalById(rentalId);

                return new RentalViewModel
                {
                    Id = rental.Id,
                    Units = rental.Units
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Get));

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] RentalBindingModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                var rentalInfo = await _rentalsService.InsertNewRentalObtainRentalId(
                    new RentalEntity
                    {
                        Units = model.Units,
                        PreprationTimeInDays = model.PreparationTimeInDays
                    });

                return rentalInfo.Item1 switch
                {
                    InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected => BadRequest("Booking not added, try again, if persist contact technical support"),
                    InsertUpdateNewRentalStatus.OK => Ok(new { id = rentalInfo.Item2 }),
                    _ => throw new InvalidOperationException($"{nameof(InsertUpdateNewRentalStatus)}: Not expected or implemented"),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Post));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
