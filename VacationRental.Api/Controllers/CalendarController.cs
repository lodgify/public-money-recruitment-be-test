using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Extensions;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] CalendarRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _calendarService.GetCalendar(request);
            return result.ToOk(e => e);
        }
    }
}
