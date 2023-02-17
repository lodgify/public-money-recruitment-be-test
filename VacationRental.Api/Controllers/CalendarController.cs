using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using VacationRental.Api.Operations.CalendarOperations;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarGetOperation _calendarGetOperation;

        public CalendarController(ICalendarGetOperation calendarGetOperation)
        {
            _calendarGetOperation = calendarGetOperation;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var result = _calendarGetOperation.ExecuteAsync(rentalId, start, nights);

            return result;
        }
    }
}
