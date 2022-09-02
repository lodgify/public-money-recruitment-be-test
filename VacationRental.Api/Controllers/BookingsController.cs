using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VR.Application.Queries.GetBooking;
using VR.Application.Requests.AddBooking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{bookingId:int}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int bookingId)
        {
            var response = await _mediator.Send(new GetBookingQuery(bookingId), HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody]AddBookingRequest request)
        {
            var response = await _mediator.Send(request, HttpContext.RequestAborted);

            return Ok(response);
        }
    }
}
