using System;
using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using VacationRental.Application.Rentals.Queries.GetRental;
using VacationRental.Application.Rentals.Commands.CreateRental;
using VacationRental.Application.Rentals.Commands.UpdateRental;
using VacationRental.Application.Rentals.Models;

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

        [HttpGet("{rentalId}")]
        public async Task<RentalViewModel> GetRental([FromRoute]GetRentalsQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> AddRental([FromBody] CreateRentalCommand command, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{rentalId}")]
        public async Task<IActionResult> UpdateRental([FromRoute] int rentalId, [FromBody] UpdateRentalCommand command, CancellationToken cancellationToken)
        {
            command.SetRentalId(rentalId);
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}

