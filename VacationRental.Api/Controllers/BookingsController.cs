using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Bookings;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Booking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;

        public BookingsController(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        /// <summary>
        /// Get booking by id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("{bookingId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<BookingResponse> Get(int bookingId)
        {
            try
            {
                var booking = _bookingsService.Get(bookingId);
                if (booking == null)
                {
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}. {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        /// <summary>
        /// Add booking
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(CreateBookingRequest request)
        {
            try
            {
                var bookingEntity = _bookingsService.Book(request);
                var resourceId = new ResourceIdViewModel
                {
                    Id = bookingEntity.Id
                };

                return Ok(resourceId);
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}. {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
