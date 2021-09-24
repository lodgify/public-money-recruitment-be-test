using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Rental;
using VacationRental.Application.Queries.Rental;
using VacationRental.Domain.Repositories;

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

        //[HttpGet]
        //[Route("{rentalId:int}")]
        //public RentalViewModel Get(int rentalId)
        //{
        //    if (!_rentals.ContainsKey(rentalId))
        //        throw new ApplicationException("Rental not found");

        //    return _rentals[rentalId];
        //}

        //[HttpPost]
        //public ResourceIdViewModel Post(RentalBindingModel model)
        //{
        //    var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

        //    _rentals.Add(key.Id, new RentalViewModel
        //    {
        //        Id = key.Id,
        //        Units = model.Units
        //    });

        //    return key;
        //}

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int id)
        {
            return await _mediator.Send(new GetRentalByIdQuery(id));
        }


        [HttpPost]
        public async Task<ResourceIdViewModel> Post(CreateRentalRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
