using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using VacationRental.Application.Rentals.Commands.PutRental;
using VacationRental.Application.Rentals.Queries.GetRental;

namespace VacationRental.Api.Controllers.Rentals
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class PutRentalController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PutRentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<ResourceIdViewModel>> Put(int rentalId, RentalBindingModel model)
        {
            try
            {
                var result = await _mediator.Send(new PutRentalCommand()
                {
                    Id = rentalId,
                    Units = model.Units,
                    PreparationTimeInDays = model.PreparationTimeInDays
                });

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
