using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Extensions;
using VacationRental.Api.Core.Interfaces;

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

        [HttpGet("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required] int bookingId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            return booking.ToOk(e => e);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> Post([Required] BookingBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _bookingService.InsertNewBookingAsync(model);
            return booking.ToOk(e => e);
        }
    }
}
