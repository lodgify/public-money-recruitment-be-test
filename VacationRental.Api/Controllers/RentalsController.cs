using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Rental;
using VacationRental.Services.Rentals;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public ActionResult<RentalViewModel> Get(int rentalId)
        {
            try
            {
                var rental = _rentalService.Get(rentalId);
                if (rental == null)
                {
                    return NotFound();
                }

                return Ok(rental);
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}. {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            try
            {
                var rental = _rentalService.Add(model);
                var result = new ResourceIdViewModel
                {
                    Id = rental.Id
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}. {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
