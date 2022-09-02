using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VR.Application.Queries.GetRental;
using VR.Application.Requests.AddRental;
using VR.Application.Requests.UpdateRental;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{rentalId:int}")]
        [ProducesResponseType(typeof(GetRentalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int rentalId)
        {
            var response = await _mediator.Send(new GetRentalQuery(rentalId), HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddRentalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AddRentalRequest request)
        {
            var response = await _mediator.Send(request, HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UpdateRentalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody] UpdateRentalRequest request)
        {
            request.Id = id;
            var resonse = await _mediator.Send(request, HttpContext.RequestAborted);

            return Ok(resonse);
        }
    }
}
