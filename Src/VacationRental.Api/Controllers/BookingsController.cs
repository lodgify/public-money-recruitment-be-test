using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Bookings.Commands.PostBooking;
using VacationRental.Application.Bookings.Queries.GetBooking;
using VacationRental.Application.Common.ViewModel;

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
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            var bookingViewModel = await _mediator.Send(new GetBookingQuery(){BookingId = bookingId});
            
            if(bookingViewModel == null)
                return NotFound("Booking not found");

            return Ok(bookingViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel model)
        {
            try
            {
                if (model.Nights <= 0)
                    return BadRequest("Nights must be positive");

                var result = await _mediator.Send(new PostBookingCommand() {Model = model});
                
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
