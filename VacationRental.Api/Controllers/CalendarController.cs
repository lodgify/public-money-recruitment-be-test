using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Queries.Calendar;
using VacationRental.Application.Queries.Calendar.ViewModel;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

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

        [HttpGet]
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            return await _mediator.Send(new BookingCalendarForRentalQuery{RentalId = rentalId, Start = start, Nights = nights});
        }
    }
}
