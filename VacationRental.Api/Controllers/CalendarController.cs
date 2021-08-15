using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Features;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            GetCalendar.Query request = new GetCalendar.Query
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };

            return _mediator.Send(request);
        }
    }
}
