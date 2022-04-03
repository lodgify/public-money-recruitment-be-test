using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Queries.GetRental;

namespace VacationRental.Api.Controllers.Rentals
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class GetRentalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetRentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            var result = await _mediator.Send(new GetRentalQuery() {RentalId = rentalId});

            if (result == null)
                return NotFound("Rental not found");

            return Ok(result);
        }
    }
}
