using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int bookingId)
        {
            var result = _bookingService.Get(bookingId);

            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post(BookingBindingModel model)
        {
            var result = _bookingService.Create(model);

            if (result == null)
                return StatusCode(500);

            return Ok(result);
        }
    }
}
