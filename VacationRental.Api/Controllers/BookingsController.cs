using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Booking;
using VacationRental.Application.Queries.Booking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _mediator.Send(new GetBookingByIdQuery(bookingId));
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Create([FromBody] BookingRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
