using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Calendar;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Calendar;

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
        public ActionResult<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            try
            {
                var calendar = _calendarService.GetCalendar(rentalId, start, nights);
                if (calendar == null)
                {
                    return NotFound();
                }

                return Ok(calendar);
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}. {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
