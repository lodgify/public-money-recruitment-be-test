using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Data;
using VacationRental.Domain.Rentals;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IEntityRepository<Rentals> _renatalsRepository;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IEntityRepository<Rentals> renatalsRepository)
        {
            _rentals = rentals;
            _renatalsRepository = renatalsRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var a = _renatalsRepository.GetEntityById(rentalId);

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            _renatalsRepository.Add(new Rentals(model.Units, model.PreparationTimeInDays));

            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTime = model.PreparationTimeInDays
            });

            return key;
        }
    }
}
