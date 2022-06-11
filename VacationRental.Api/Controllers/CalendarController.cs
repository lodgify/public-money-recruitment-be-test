using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        private readonly IBookingsService _bookingsService1;

        public CalendarController(
            IRentalsService rentalsService,
            IBookingsService bookingsService)
        {
            _rentalsService = rentalsService;
            _bookingsService1 = bookingsService;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalsService.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return _bookingsService1.GetBookingCalendar(rental, start, nights);
        }
    }
}
