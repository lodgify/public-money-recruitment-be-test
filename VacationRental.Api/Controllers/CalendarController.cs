using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

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
        [Route("{rentalId:int}&{start:datetime}&{nights:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CalendarViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int rentalId, DateTime start, int nights)
        {
            var result = _calendarService.GetCalendar(rentalId, start, nights);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
