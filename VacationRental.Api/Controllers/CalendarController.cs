using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Utilities;
using VacationRental.Business.Abstract;
using VacationRental.Domain.DTOs;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService calendarservice;
        private readonly IRentalService rentalService;

        public CalendarController(ICalendarService _calendarservice, IRentalService _rentalService)
        {
            calendarservice = _calendarservice;
            rentalService = _rentalService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] CalendarDto model)
        { 
            var bookings = calendarservice.GetBookings(model);

            if (bookings is null)
            {
                return NotFound("Rental not found");
            }

            var builder = new CalendarViewBuilder();

            var preparationTime = rentalService.GetById(model.rentalId).PreparationTimeInDays;

            var result = builder.CalendarView(model, bookings, preparationTime);

            return Ok(result);
        }
    }
}
