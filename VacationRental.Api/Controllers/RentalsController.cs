using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Presenter.Interfaces;
using VacationRental.Application.Handlers.Rentals;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IVacationRentalPresenter _presenter;

        public RentalsController(IMediator mediator, IVacationRentalPresenter presenter)
        {
            _mediator = mediator;
            _presenter = presenter;
        }

        [ProducesResponseType(200, Type = typeof(RentalViewModel))]
        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> Get(int rentalId)
        {
            return _presenter.GetResult(await _mediator.Send(new GetRentalRequest(rentalId)));
        }

        [ProducesResponseType(200, Type = typeof(ResourceIdViewModel))]
        [HttpPost]
        public async Task<IActionResult> Post(RentalBindingModel model)
        {
            return _presenter.GetResult(await _mediator.Send(new CreateRentalRequest(model.Units, model.PreparationTimeInDays)));
        }

        [ProducesResponseType(200, Type = typeof(RentalViewModel))]
        [HttpPut]
        public async Task<IActionResult> Put(int id, int units, int preparation)
        {
            return _presenter.GetResult(await _mediator.Send(new UpdateRentalRequest(id, units, preparation)));
        }
    }
}
