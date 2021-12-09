using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace RentalSoftware.API.Controllers
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
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            var response = await _calendarService.GetCalendar(new GetCalendarRequest() { RentalId = rentalId, BookingStartDate = start, NumberOfNights = nights });

            if (!response.Succeeded)
            {
                throw new Exception(response.Message);
            }

            return response.CalendarViewModel;
        }
    }
}
