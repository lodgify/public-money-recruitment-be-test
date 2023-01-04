using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalsController(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalDto Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceId Post(RentalRequest model)
        {
            var key = new ResourceId { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, Rental.Create(model.Units, model.PreparationTimeInDays));

            return key;
        }
    }
}
