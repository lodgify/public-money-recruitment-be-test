using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calenderService;

        public CalendarController(ICalendarService calenderService)
        {
            _calenderService = calenderService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Successfully found and returned the Calender </response>
        /// <response code="400">The request did not pass validation checks </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(CalendarViewModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ReserveBindingModel model) =>
            Ok(await _calenderService.GetCalendar(model));
    }
}
