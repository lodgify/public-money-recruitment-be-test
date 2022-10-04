using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using VacationRental.Application.Calendars.Queries.GetCalendar;
using VacationRental.Application.Calendars.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CalendarViewModel> Get([FromQuery] GetCalendarQuery query, CancellationToken cancellationToken )
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}