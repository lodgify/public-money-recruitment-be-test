using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Features;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public Task<RentalViewModel> Get(int rentalId)
        {
            return _mediator.Send(new GetRental.Query { Id = rentalId });
        }

        [HttpPost]
        public Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            CreateRental.Command request = new CreateRental.Command
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            return _mediator.Send(request);
        }
    }
}
