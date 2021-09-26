using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Rental;
using VacationRental.Application.Queries.Rental;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            return await _mediator.Send(new GetRentalByIdQuery(rentalId));
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post([FromBody] CreateRentalRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task Put(int rentalId, [FromBody] UpdateRentalModel request)
        {
            await _mediator.Send(new UpdateRentalRequest
                {Id = rentalId, Units = request.Units, PreparationTimeInDays = request.PreparationTimeInDays});
        }
    }
}
