using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.BusinessLogic.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarsService _calendarsService;
        private readonly IMapper _mapper;
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            ICalendarsService calendarsService,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _calendarsService = calendarsService;
            _mapper = mapper;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var calendar = _calendarsService.GetCalendar(rentalId, start, nights);
            var calendarViewModel = _mapper.Map<CalendarViewModel>(calendar);

            return calendarViewModel;
        }
    }
}
