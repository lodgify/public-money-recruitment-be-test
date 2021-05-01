using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Filters.Common;
using VacationRental.Api.Filters.Rentals;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    /* 
        Since we don't have api/v1/~VACATIONRENTAL~/rentals endpoint and 
        modifying the API routes wasn't allowed I'll modify `api/v1/rentals`
     */


    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        [HttpPost]
        [RentalBindingModelFilter]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }


        [HttpGet("{rentalId:int}")]
        [ZeroFilter("rentalId")] 
        [RentalNotFountFilter("rentalId")]
        public RentalViewModel Get(int rentalId)
        {
            return _rentals[rentalId];
        }

        [HttpPut("{rentalId:int}")]
        [ZeroFilter("rentalId"), RentalNotFountFilter("rentalId")]
        [RentalBindingModelFilter]
        public RentalViewModel Put(int rentalId, [FromBody] RentalBindingModel model) 
        {
            int id = _rentals[rentalId].Id;
            _rentals[rentalId] = new RentalViewModel 
            {
                Id = id,
                Units = model.Units, 
                PreparationTimeInDays = model.PreparationTimeInDays
            };
            return _rentals[rentalId];
        }
    }
}
