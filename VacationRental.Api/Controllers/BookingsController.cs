using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Presenter.Interfaces;
using VacationRental.Application.Handlers.Bookings;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IVacationRentalPresenter _presenter;

        public BookingsController(
            IMediator mediator,
            IVacationRentalPresenter presenter)
        {
            _mediator = mediator;
            _presenter = presenter;
        }

        [ProducesResponseType(200, Type = typeof(BookingViewModel))]
        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<IActionResult> Get(int bookingId)
        {
            return _presenter.GetResult(await _mediator.Send(new GetBookingRequest(bookingId)));
        }

        [ProducesResponseType(200, Type = typeof(ResourceIdViewModel))]
        [HttpPost]
        public async Task<IActionResult> Post(BookingBindingModel model)
        {
            return _presenter.GetResult(await _mediator.Send(new CreateBookingRequest(model.Start, model.Nights, model.RentalId)));
        }
    }
}
