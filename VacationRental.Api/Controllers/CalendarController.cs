using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;
using VacationRental.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Authorize,
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

        //private readonly IDictionary<int, RentalViewModel> _rentals;
        //private readonly IDictionary<int, BookingViewModel> _bookings;

        //public CalendarController(
        //    IDictionary<int, RentalViewModel> rentals,
        //    IDictionary<int, BookingViewModel> bookings)
        //{
        //    _rentals = rentals;
        //    _bookings = bookings;
        //}

        //[HttpGet]
        //public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        //{
        //    if (nights < 0)
        //        throw new ApplicationException("Nights must be positive");
        //    if (!_rentals.ContainsKey(rentalId))
        //        throw new ApplicationException("Rental not found");

        //    var result = new CalendarViewModel 
        //    {
        //        RentalId = rentalId,
        //        Dates = new List<CalendarDateViewModel>() 
        //    };
        //    for (var i = 0; i < nights; i++)
        //    {
        //        var date = new CalendarDateViewModel
        //        {
        //            Date = start.Date.AddDays(i),
        //            Bookings = new List<CalendarBookingViewModel>()
        //        };

        //        foreach (var booking in _bookings.Values)
        //        {
        //            if (booking.RentalId == rentalId
        //                && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
        //            {
        //                date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
        //            }
        //        }

        //        result.Dates.Add(date);
        //    }

        //    return result;
        //}
    }
}
