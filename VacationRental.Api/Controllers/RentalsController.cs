using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Application.Features.Rentals.Queries.GetRental;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalDto> Get(int rentalId)
        {
            var query = new GetRentalQuery(rentalId);
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ResourceId> Post(RentalRequest model)
        {
            var command = new CreateRentalCommand(model.Units, model.PreparationTimeInDays);            
            return await _mediator.Send(command);
        }
    }
}
