using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Models.Rental;
using VacationRental.Services.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rentalService"></param>
        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        /// <summary>
        /// Retrieve all available rental information
        /// </summary>
        /// <returns>The rental information</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<RentalDto> GetRentals()
        {
            var rentals = _rentalService.GetRentals();

            return Ok(rentals);
        }

        /// <summary>
        /// Retrieve the rental information for the given rental id
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{rentalId:int}", Name = nameof(GetRentalBy))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RentalDto> GetRentalBy(int rentalId)
        {
            var rental = _rentalService.GetRentalBy(rentalId);
           
            return Ok(rental);
        }

        /// <summary>
        /// Create the host rental 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<RentalDto> AddRental(CreateRentalRequest model)
        {
            var rental = _rentalService.AddRental(model);

            return Created(Url.Link(nameof(GetRentalBy), new { rentalId = rental.Id }), rental);
        }

        /// <summary>
        /// Update existing rental
        /// </summary>
        /// <returns></returns>
        [HttpPut("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<RentalDto> UpdateRental([FromRoute] int rentalId, [FromBody] CreateRentalRequest parameters)
        {
            var rental = _rentalService.UpdateRental(rentalId, parameters);

            return Created(Url.Link(nameof(GetRentalBy), new { rentalId = rental.Id }), rental);
        }

        /// <summary>
        /// Delete existing rental
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult DeleteRental([FromRoute] int rentalId)
        {
            if (_rentalService.DeleteRental(rentalId))
            {
                return NoContent();
            }

            return Conflict();
        }
    }
}
