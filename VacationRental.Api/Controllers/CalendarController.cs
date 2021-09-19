using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        
        [HttpGet]
        public IActionResult Get([FromQuery]CalendarBindingModel model)
        {            
            if (!_rentals.ContainsKey(model.RentalId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Rental not found");
            }

            var bookings = _bookings.Where(b => b.Value.RentalId == model.RentalId);
            if (!bookings.Any())
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Bookings not found");
            }

            var result = new CalendarViewModel
            {
                RentalId = model.RentalId,
                Dates = new ()
            };

            var strartBookingDayThreshold = bookings.Min(b => b.Value.Start);
            var endBookingDateThreshold = bookings.Max(b => b.Value.EndPreperations);
            while (strartBookingDayThreshold <= endBookingDateThreshold)
            {
                strartBookingDayThreshold = strartBookingDayThreshold.AddDays(1);
                var date = new CalendarDateViewModel
                {
                    Date = strartBookingDayThreshold,
                    Bookings = new (),
                    Preparations = new ()                    
                };

                foreach (var booking in bookings)
                {
                    if (booking.Value.Start <= date.Date && booking.Value.End > date.Date)
                    {
                        date.Bookings.Add(new () { Id = booking.Value.Id, Units = booking.Value.Units });
                    }

                    if (booking.Value.End <= date.Date && booking.Value.EndPreperations > date.Date)
                    {
                        date.Preparations.Add(new { Units = booking.Value.Units });
                    } 
                }
                result.Dates.Add(date);
            }
            return Ok(result);
        }
    }
}
