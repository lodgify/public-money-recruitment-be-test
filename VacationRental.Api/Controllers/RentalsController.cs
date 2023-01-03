using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain.Primitives;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalsController(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public Rental Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceId Post(RentalDto model)
        {
            var key = new ResourceId { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new Rental
            {
                Id = key.Id,
                Units = model.Units
            });

            return key;
        }
    }
}
