using System.Threading.Tasks;
using Application.Business.Queries.Calendar;
using Application.Models.Calendar.Requests;
using Application.Models.Calendar.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<CalendarViewModel> Get([FromQuery] GetCalendarRequest request)
        {
            return await _mediator.Send(new GetCalendarQuery(request));
        }
    }
}