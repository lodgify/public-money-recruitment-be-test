
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Bookings.Queries.GetBooking;

namespace VacationRental.Api.Controllers.Bookings
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class GetBookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetBookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            var bookingViewModel = await _mediator.Send(new GetBookingQuery(){BookingId = bookingId});
            
            if(bookingViewModel == null)
                return NotFound("Booking not found");

            return Ok(bookingViewModel);
        }
    }
}
