using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services.Interfaces;

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
        public ActionResult<CalendarDTO> Get(int rentalId, DateTime start, int nights)
        {
            var result = _calendarService.GetCalendar(rentalId, start, nights);

            return result;
        }
    }
}
