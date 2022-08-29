using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Bookings;
using VacationRental.Services.Models.Booking;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bookingsService"></param>
        public BookingsController(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        /// <summary>
        /// Retrieve all available booking information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<BookingDto> GetBookings()
        {
            var bookings = _bookingsService.GetBookings();

            return Ok(bookings);
        }

        /// <summary>
        /// Retrieve the booking information for the given booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("{bookingId:int}", Name = nameof(GetBookingById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<BookingDto> GetBookingById(int bookingId)
        {
            var booking = _bookingsService.GetBookingById(bookingId);
            
            return Ok(booking);
        }

        /// <summary>
        /// Create new booking for the existing rental 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<BookingDto> AddBooking(CreateBookingRequest request)
        {
            var booking = _bookingsService.AddBooking(request);

            return Created(Url.Link(nameof(GetBookingById), new { bookingId = booking.Id }), booking);
        }

        /// <summary>
        /// Update existing booking for the existing rental
        /// </summary>
        /// <returns></returns>
        [HttpPut("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<BookingDto> UpdateBooking([FromRoute] int bookingId, [FromBody] CreateBookingRequest request)
        {
            var booking = _bookingsService.UpdateBooking(bookingId, request);

            return Created(Url.Link(nameof(GetBookingById), new { bookingId = booking.Id }), booking);
        }

        /// <summary>
        /// Delete existing booking
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult DeleteBooking([FromRoute] int bookingId)
        {
            if (_bookingsService.DeleteBooking(bookingId))
            {
                return NoContent();
            }

            return Conflict();
        }
    }
}
