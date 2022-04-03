using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Calendars.Queries.GetCalendar;

namespace VacationRental.Api.Controllers.Calendars
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class GetCalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetCalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                return BadRequest("Nights must be positive");

            var result = await _mediator.Send(new GetCalendarQuery()
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            });

            if (result == null)
                return NotFound("Rental not found");
            
            return Ok(result);
        }
    }
}
