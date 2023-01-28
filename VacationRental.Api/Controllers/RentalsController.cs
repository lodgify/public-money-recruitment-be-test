using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Dtos;
using VacationRental.Application.Midlewares.Rental;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalMiddleware _rentalMiddleware;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IRentalMiddleware rentalMiddleware)
        {
            _rentals = rentals;
            this._rentalMiddleware = rentalMiddleware;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var rentalDto = new RentalDto(model.Units, model.PreparationTimeInDays);
            var response = await _rentalMiddleware.AddRentalWithTimePeriod(rentalDto);

            _rentals.Add(response, new RentalViewModel
            {
                Id = response,
                Units = model.Units
            });

            var resource = new ResourceIdViewModel();
            resource.Id = response;
            return resource;
        }
	}
}
