using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;
using VacationRental.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Authorize,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"),
     ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;

        public RentalsController(IRentalService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get rentals
        /// </summary>
        /// <returns></returns>
        [HttpGet,
         ProducesResponseType(typeof(IEnumerable<RentalDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsAsync()
        {
            var result = await _service.GetRentalsAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get rental by id
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("{rentalId:int}"),
         ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RentalDto>> GetRentalByIdAsync([FromRoute] int rentalId) 
        {
            var result = await _service.GetRentalByIdAsync(rentalId);

            return Ok(result);
        }

        /// <summary>
        /// Add rental
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost,
         ProducesResponseType(typeof(BaseEntityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseEntityDto>> AddRentalAsync([FromBody] RentalParameters parameters)
        {
            var result = await _service.AddRentalAsync(parameters);

            return Ok(result);
        }

        /// <summary>
        /// Update rental
        /// </summary>
        /// <returns></returns>
        [HttpPut("{rentalId:int}"),
         ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRentalAsync([FromRoute] int rentalId, [FromBody] RentalParameters parameters)
        {
            await _service.UpdateRentalAsync(rentalId, parameters);

            return NoContent();
        }

        /// <summary>
        /// Delete rental
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{rentalId:int}"),
         ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRentalAsync([FromRoute] int rentalId)
        {
            await _service.DeleteRentalAsync(rentalId);

            return NoContent();
        }
    }
}
