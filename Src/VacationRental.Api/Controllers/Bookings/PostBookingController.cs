using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Bookings.Commands.PostBooking;
using VacationRental.Application.Bookings.Queries.GetBooking;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Api.Controllers.Bookings
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class PostBookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostBookingController(IMediator mediator)
        {
            _mediator = mediator;
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
