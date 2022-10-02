using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService paramBookingService)
        {
            _bookingService = paramBookingService;
        }

        /// <summary>
        ///  This method is responsible to return the booking information.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Return a object BookingViewModel</returns>
        /// <response code="200">BookingViewModel</response>
        /// <response code="404">The booking with the parameters passed does not exist.</response> 
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(BookingViewModel), 200)]
        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _bookingService.Get(bookingId);
        }

        /// <summary>
        ///  This method is responsible to booking.
        /// </summary>
        /// <param name="bookingModel"></param>
        /// <returns>Return a object ResourceIdViewModel</returns>
        /// <response code="200">ResourceIdViewModel</response>
        /// <response code="404">The rental with the parameters passed does not exist.</response> 
        /// <response code="409">Could be exists conflict with the number of nights or the rental could not be available.</response> 
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(ResourceIdViewModel), 200)]
        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel bookingModel)
        {
            return await _bookingService.Post(bookingModel);
        }
    }
}
