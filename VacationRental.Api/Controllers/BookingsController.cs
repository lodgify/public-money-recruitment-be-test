using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Bookings;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingsController : ControllerBase
    {
        private IMediator _mediator;
        
        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingDto Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public ResourceId Post(BookingRequest request)
        {
            var command = new CreateBookingCommand(request.RentalId, request.Start, request.Nights, request.Units);
            return _mediator.Send(command).Result;
        }
    }
}
