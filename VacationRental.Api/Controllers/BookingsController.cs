using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Returns a single booking by id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <response code="200">Successfully found and returned the booking </response>
        /// <response code="400">The booking does not exist in the system  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(RentalViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("{bookingId:int}")]
        public IActionResult Get(int bookingId) => Ok(_bookingService.GetBooking(bookingId));

        /// <summary>
        /// Creates a new booking in the system
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">booking was successfully created </response>
        /// <response code="400">The request did not pass validation checks </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ResourceIdViewModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Post(BookingBindingModel model) =>
            Ok(await _bookingService.CreateAsync(model));

        // for (var i = 0; i < model.Nights; i++)
        // {
        //     var count = 0;
        //     foreach (var booking in _bookings.Values)
        //     {
        //         if (booking.RentalId == model.RentalId
        //             && (booking.Start <= model.Start.Date &&
        //                 booking.Start.AddDays(booking.Nights) > model.Start.Date)
        //             || (booking.Start < model.Start.AddDays(model.Nights) &&
        //                 booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
        //             || (booking.Start > model.Start &&
        //                 booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
        //         {
        //             count++;
        //         }
        //     }
        //
        //     if (count >= _rentals[model.RentalId].Units)
        //         throw new ApplicationException("Not available");
        // }
    }
}
