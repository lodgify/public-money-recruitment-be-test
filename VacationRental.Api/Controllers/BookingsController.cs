using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using VacationRental.Application.Bookings.Models;
using VacationRental.Application.Bookings.Queries.GetBooking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{bookingId}")]
        public async Task<BookingViewModel> Get([FromRoute]GetBookingsQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
