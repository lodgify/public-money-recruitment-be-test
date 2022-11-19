using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Queries;
using VacationRental.Shared.Abstractions.Dispatchers;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CalendarController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            var calendar = await _dispatcher.QueryAsync(new GetCalendar { RentalId = rentalId, Start = start, Nights = nights });

            return Ok(calendar.AsViewModel());
        }
    }
}
