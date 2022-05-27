using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Presenter.Interfaces;
using VacationRental.Application.Handlers.Calendar;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IVacationRentalPresenter _presenter;

        public CalendarController(
            IMediator mediator, IVacationRentalPresenter presenter)
        {
            _mediator = mediator;
            _presenter = presenter;
        }

        [ProducesResponseType(200, Type = typeof(CalendarViewModel))]
        [HttpGet]
        public async Task<IActionResult> Get(int rentalId, DateTime start, int nights)
        {
            return _presenter.GetResult(await _mediator.Send(new GetCalendarRequest(rentalId, start, nights)));
        }
    }
}
