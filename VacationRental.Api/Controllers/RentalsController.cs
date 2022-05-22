using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    //[Route("api/v1")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        
        private readonly IDictionary<int, BookingViewModel> _bookings;


        public RentalsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }


        [HttpGet]
        [Route("{rentalId:int}")]
        //[Route("rentals/{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        //[Route("vacationrental/rentals")]
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

        [HttpPut]
        [Route("{rentalId:int}")]
        //[Route("rentals/{rentalId:int}")]
        public UpdateRentalResultViewModel Put(int rentalId, [FromBody] RentalBindingModel model)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[rentalId];

            if(HasOverLap(rental, model))
            {
                return new UpdateRentalResultViewModel(){ updated = false};
            }
            else
            {
                var RentalToInsert = new RentalViewModel
                {
                    Id = rental.Id,
                    Units = model.Units,
                    PreparationTimeInDays = model.PreparationTimeInDays
                };

                _rentals[rental.Id] = RentalToInsert;
            }

            return new UpdateRentalResultViewModel() { updated = true };
        }
        private bool HasOverLap(RentalViewModel rental, RentalBindingModel model)
        {
            var rentalBookingsGroupedByUnit =
                _bookings.Values.Where(booking => booking.RentalId == rental.Id)
                                .GroupBy(booking => booking.Unit);

            if (rentalBookingsGroupedByUnit.Count() > model.Units)
            {
                return true;
            }

            if (rental.PreparationTimeInDays < model.PreparationTimeInDays)
            {
                foreach (var rentalBookingUnit in rentalBookingsGroupedByUnit)
                {
                    var rentalBookingList = rentalBookingUnit.ToList();
                    for (int i= 0; i < rentalBookingList.Count() - 1; ++i)
                    {
                        var firstItemEnd = rentalBookingList[i].Start.AddDays(rentalBookingList[i].Nights + model.PreparationTimeInDays);
                        var secondItemStart = rentalBookingList[i + 1].Start;

                        if(firstItemEnd > secondItemStart)
                        {
                            return true;
                        }                    
                    }
                }
            }

            return false;
        }
    }
}
