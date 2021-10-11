using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VacationRental.Booking.DTOs;
using VacationRental.Booking.Services;

namespace VacationRental.Booking.Adapters
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _service;
        private readonly ILogger<BookingsController> _logger;


        public BookingsController(BookingService service, ILogger<BookingsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> Get(int Id, [FromQuery] GetBookingRequest request)
        {

            if (Id != 0 && request.Id == 0)
            {
                request.Id = Id;
            }
            var Booking = await _service.SearchAsync(request);
            return Ok(Booking);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddBookingRequest model)
        {
            var rental = await _service.AddNewAsync(model);
            return Ok(rental);
        }
    }
}
