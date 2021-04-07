using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        //private readonly IDictionary<int, RentalViewModel> _rentals;
        //private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IRentalsRepository _rentalsRepository;
        private readonly ICalendarService _calendarService;

        public CalendarController(
            //IDictionary<int, RentalViewModel> rentals,
            //IDictionary<int, BookingViewModel> bookings,
            IRentalsRepository rentalsRepository,
            ICalendarService calendarService)
        {
            //_rentals = rentals;
            //_bookings = bookings;
            _rentalsRepository = rentalsRepository;
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            try
            {
                if (nights < 0)
                    return BadRequest("Nights must be positive");
                //throw new ApplicationException("Nights must be positive");
                //if (!_rentals.ContainsKey(rentalId))
                if (!await _rentalsRepository.RentalExists(rentalId))
                    return NotFound("Rental not found");
                //throw new ApplicationException("Rental not found");

                var calendarModel = await _calendarService.GetRentalCalendarByNights(rentalId, start, nights);

                return new CalendarViewModel
                {
                    RentalId = calendarModel.RentalId,
                    Dates = calendarModel.Dates.Select(a => 
                    new CalendarDateViewModel 
                    { 
                        Bookings = a.Bookings.Select(b => new CalendarBookingViewModel { Id = b.Id, Unit = b.Unit }).ToList(),
                        Date = a.Date,
                        PreparationTime = a.PreparationTimes.Select(b => new PreparationTimesViewModel { Unit = b.Unit }).ToList()
                    }).ToList(),
                };

                //var result = new CalendarViewModel
                //{
                //    RentalId = rentalId,
                //    Dates = new List<CalendarDateViewModel>()
                //};
                //for (var i = 0; i < nights; i++)
                //{
                //    var date = new CalendarDateViewModel
                //    {
                //        Date = start.Date.AddDays(i),
                //        Bookings = new List<CalendarBookingViewModel>()
                //    };

                //    foreach (var booking in _bookings.Values)
                //    {
                //        if (booking.RentalId == rentalId
                //            && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                //        {
                //            date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                //        }
                //    }

                //    result.Dates.Add(date);
                //}

                //return result;
            }
            catch (Exception ex) 
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
