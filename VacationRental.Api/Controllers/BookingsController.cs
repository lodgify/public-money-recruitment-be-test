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
    [//Authorize,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"),
     ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get bookings
        /// </summary>
        /// <returns></returns>
        [HttpGet,
         ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsAsync()
        {
            var result = await _service.GetBookingsAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get booking by id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("{bookingId:int}"),
         ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingDto>> GetBookingByIdAsync([FromRoute] int bookingId)
        {
            var result = await _service.GetBookingByIdAsync(bookingId);
            
            return Ok(result);
        }

        /// <summary>
        /// Add booking
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost,
         ProducesResponseType(typeof(BaseEntityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseEntityDto>> AddBookingAsync([FromBody] BookingParameters parameters)
        {
            var result = await _service.AddBookingAsync(parameters);

            return Ok(result);
        }

        /// <summary>
        /// Update booking
        /// </summary>
        /// <returns></returns>
        [HttpPut("{bookingId:int}"),
         ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateBookingAsync([FromRoute] int bookingId, [FromBody] BookingParameters parameters)
        {
            await _service.UpdateBookingAsync(bookingId, parameters);

            return NoContent();
        }

        /// <summary>
        /// Delete booking
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{bookingId:int}"),
         ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBookingAsync([FromRoute] int bookingId)
        {
            await _service.DeleteBookingAsync(bookingId);

            return NoContent();
        }
    }
}
