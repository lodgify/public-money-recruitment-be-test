using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Calendar;
using VacationRental.Services.Models.Calendar;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="calendarService"></param>
        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        /// <summary>
        /// Retrieve the booking information for the given rental id, start date, number of nights
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CalendarDto> Get(int rentalId, DateTime start, int nights)
        {
            var calendar = _calendarService.GetCalendar(rentalId, start, nights);

            return Ok(calendar);
        }
    }
}
