using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

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
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            if (start == default)
                throw new ApplicationException("Start date is not applicable!");

            if (nights <= 0)
                throw new ApplicationException("Nights must be greater than zero!");

            CalendarBindingModel calendarServiceRequestModel = new CalendarBindingModel()
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };

            return _calendarService.Get(calendarServiceRequestModel);
        }
    }
}
