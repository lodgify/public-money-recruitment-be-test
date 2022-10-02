using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService paramCalendarService)
        {
            _calendarService = paramCalendarService;
        }

        /// <summary>
        ///  This method is responsible to return the calendar with rentals information.
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <returns>Return a object CalendarViewModel</returns>
        /// <response code="200">CalendarViewModel</response>
        /// <response code="409">The rental/booking with the parameters passed is not available.</response> 
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(CalendarViewModel), 200)]

        [HttpGet]
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            return await _calendarService.Get(rentalId, start, nights);
        }
    }
}
