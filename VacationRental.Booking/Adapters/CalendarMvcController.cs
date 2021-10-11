using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VacationRental.Booking.DTOs;
using VacationRental.Booking.Services;

namespace VacationRental.Booking.Adapters
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly BookingService _service;
        private readonly ILogger<CalendarController> _logger;


        public CalendarController(BookingService service, ILogger<CalendarController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int rentalId, DateTime start, int nights, [FromQuery] GetCalendarRequest request)
        {
            if (rentalId == 0 && request.Id == 0)
            {
                throw new ApplicationException("Invalid Id");
            }
            else if (request.RentalId == 0 && rentalId != 0)
            {
                request.RentalId = rentalId;
                request.Start = start;
                request.Nights = nights;
            }


            var calendar = await _service.GetCalendarAsync(request);
            return Ok(calendar);
        }


    }
}
