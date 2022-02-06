using System.Threading.Tasks;
using Application.Business.Commands.Booking;
using Application.Business.Queries.Booking;
using Application.Models;
using Application.Models.Booking.Requests;
using Application.Models.Booking.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingResponse> Get(int bookingId)
        {
            return await _mediator.Send(new GetBookingQuery(bookingId));
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(CreateBookingRequest model)
        {
            return await _mediator.Send(new CreateBookingCommand(model));
        }
    }
}