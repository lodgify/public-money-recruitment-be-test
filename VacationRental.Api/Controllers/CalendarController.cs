using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Business.BusinessLogic;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {

        private readonly CalendarBusinessLogic _calendarBusinessLogic;
        public CalendarController(CalendarBusinessLogic calendarBusinessLogic)
        {
            _calendarBusinessLogic = calendarBusinessLogic;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var result = _calendarBusinessLogic.GetRentalCalendar(rentalId, start, nights);

            return result;
        }
    }
}
