using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using VacationRental.Application.Rentals.Queries.GetRental;

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
        public async Task<RentalViewModel> Get(int rentalId)
        {
            var result = await _mediator.Send(new GetRentalQuery() {RentalId = rentalId});

            if (result == null)
                throw new ApplicationException("Rental not found");

            return result;
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var result = await _mediator.Send(new PostRentalCommand()
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return result;
        }
    }
}
