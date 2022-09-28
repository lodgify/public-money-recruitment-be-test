using System;
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

        public CalendarController(ICalendarsService calendarsService,
            IMapper mapper)
        {
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
