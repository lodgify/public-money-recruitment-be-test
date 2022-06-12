using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain.Models;
using VacationRental.Services.IServices;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {

        private readonly ICalenderService _calendarService;


        public CalendarController(ICalenderService calenderService)
        {
            _calendarService = calenderService;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            CalendarViewModel calendar = _calendarService.Get(rentalId, start, nights);

            return calendar;
        }

        [HttpGet]
        [Route("~/api/v1/vacationrental/calendar")]
        public CalendarViewModel VacationrentalGet(int rentalId, DateTime start, int nights)
        {

            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            CalendarViewModel calendar = _calendarService.GetVacationrental(rentalId, start, nights);

            return calendar;

        }
    }
}
