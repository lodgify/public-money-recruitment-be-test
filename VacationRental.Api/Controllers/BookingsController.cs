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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get booking by id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("{bookingId:int}"),
         ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingDto>> GetBookingByIdAsync([FromRoute] int bookingId)
        {
            var result = await _service.GetBookingByIdAsync(bookingId);
            
            return Ok(result);
        }

        /// <summary>
        /// Add booking
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost,
         ProducesResponseType(typeof(BaseEntityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseEntityDto>> AddBookingAsync([FromBody] BookingParameters parameters)
        {
            var result = await _service.AddBookingAsync(parameters);

            return Ok(result);
        }

    //    private readonly IDictionary<int, RentalViewModel> _rentals;
    //    private readonly IDictionary<int, BookingViewModel> _bookings;

    //    public BookingsController(
    //        IDictionary<int, RentalViewModel> rentals,
    //        IDictionary<int, BookingViewModel> bookings)
    //    {
    //        _rentals = rentals;
    //        _bookings = bookings;
    //    }

    //    [HttpGet]
    //    [Route("{bookingId:int}")]
    //    public BookingViewModel Get(int bookingId)
    //    {
    //        if (!_bookings.ContainsKey(bookingId))
    //            throw new ApplicationException("Booking not found");

    //        return _bookings[bookingId];
    //    }

    //    [HttpPost]
    //    public ResourceIdViewModel Post(BookingBindingModel model)
    //    {
    //        if (model.Nights <= 0)
    //            throw new ApplicationException("Nigts must be positive");
    //        if (!_rentals.ContainsKey(model.RentalId))
    //            throw new ApplicationException("Rental not found");

    //        for (var i = 0; i < model.Nights; i++)
    //        {
    //            var count = 0;
    //            foreach (var booking in _bookings.Values)
    //            {
    //                if (booking.RentalId == model.RentalId
    //                    && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
    //                    || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
    //                    || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
    //                {
    //                    count++;
    //                }
    //            }
    //            if (count >= _rentals[model.RentalId].Units)
    //                throw new ApplicationException("Not available");
    //        }


    //        var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

    //        _bookings.Add(key.Id, new BookingViewModel
    //        {
    //            Id = key.Id,
    //            Nights = model.Nights,
    //            RentalId = model.RentalId,
    //            Start = model.Start.Date
    //        });

    //        return key;
    //    }
    }
}
