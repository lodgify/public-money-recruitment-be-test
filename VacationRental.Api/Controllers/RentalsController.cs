using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int rentalId)
        {
            var result = _rentalService.Get(rentalId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post(RentalBindingModel model)
        {
            var result = _rentalService.Create(model);

            if (result == null)
                return StatusCode(500);

            return Ok(result);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public IActionResult Put(int id, RentalBindingModel model)
        {
            _rentalService.Update(id, model);

            return NoContent();
        }
    }
}
