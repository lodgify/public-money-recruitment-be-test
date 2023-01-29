using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Midlewares.Calendar;
using VacationRental.Application.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarMiddleware _calendarMiddleware;

        public CalendarController(ICalendarMiddleware calendarMiddleware)
        {
            this._calendarMiddleware = calendarMiddleware;
        }

		[HttpGet]
        public async Task<CalendarViewModel> Get(CalendarInputViewModel input)
        {
            var response = await this._calendarMiddleware.GetAvailableCalendar(input);
            return response;
        }
    }
}
