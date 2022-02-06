using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Business.Commands.Rental;
using Application.Business.Queries.Rental;
using Application.Models;
using Application.Models.Rental.Requests;
using Application.Models.Rental.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<RentalResponse> Get(int rentalId)
        {
            return await _mediator.Send(new GetRentalQuery(rentalId));
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(CreateRentalRequest model)
        {
            return await _mediator.Send(new CreateRentalCommand(model));
        }
        
        [HttpPut]
        public async Task Put(UpdateRentalRequest request, CancellationToken token)
        {
            await _mediator.Send(new UpdateRentalCommand(request), token);
        }
    }
}