using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;

namespace VacationRental.Api.Controllers.Rentals
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class PostRentalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostRentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(RentalBindingModel model)
        {
            var result = await _mediator.Send(new PostRentalCommand()
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return Ok(result);
        }
        
    }
}
