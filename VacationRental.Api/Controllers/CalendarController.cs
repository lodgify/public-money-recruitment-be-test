using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;
using VacationRental.Domain.Aggregates.Calendars;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;        

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public CalendarDto Get(int rentalId, DateTime start, int nights)
        {
            var query = new GetRentalCalendarQuery(rentalId, start, nights);
            return _mediator.Send(query).Result;
        }
    }
}
