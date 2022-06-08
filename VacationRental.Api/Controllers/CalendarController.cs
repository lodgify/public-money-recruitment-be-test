using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;
using VacationRental.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [//Authorize,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"),
     ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _service;

        public CalendarController(ICalendarService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get calendar
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet, 
         ProducesResponseType(typeof(CalendarDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CalendarDto>> GetCalendarAsync([FromQuery] GetCalendarParameters parameters)
        {
            var result = await _service.GetCalendarAsync(parameters.RentalId.Value, parameters.Nights.Value, parameters.Start.Value);

            return Ok(result);
        }
    }
}
