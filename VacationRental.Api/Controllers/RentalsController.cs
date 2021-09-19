using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public IActionResult Get(int rentalId)
        {            
            try
            {
                if (!_rentals.ContainsKey(rentalId))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Rental not found");
                }
                return Ok(_rentals[rentalId]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(RentalBindingModel model)
        {
            try
            {
                var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };
                _rentals.Add(key.Id, new RentalViewModel
                {
                    Id = key.Id,
                    Units = model.Units,
                    PreparationTimeInDays = model.PreparationTimeInDays
                });
                return Ok(key);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(RentalBindingModel model)
        {
            try
            {
                if (!_rentals.ContainsKey(model.Id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Rental or booking not found");
                }

                var bookingsWorkspace = _bookings.Where(b => b.Value.RentalId == model.Id).ToDictionary(k => k.Key, v => v.Value);

                if (!bookingsWorkspace.Any())
                {                    
                    _rentals[model.Id].PreparationTimeInDays = model.PreparationTimeInDays;
                    _rentals[model.Id].Units = model.Units;
                    return Ok(new ResourceIdViewModel { Id = model.Id });
                }

                foreach (var bookingWorkspace in bookingsWorkspace)
                {
                    var bookingCopy = _bookings[bookingWorkspace.Key];
                    _bookings.Remove(bookingWorkspace.Key);

                    bookingsWorkspace.Remove(bookingWorkspace.Key);
                    bookingWorkspace.Value.PreparationTimeInDays = model.PreparationTimeInDays;
                    bookingWorkspace.Value.Units = model.Units;

                    var isOverlappingCheck = false;
                    foreach(var bookingCheck in bookingsWorkspace)
                    {
                        if(bookingWorkspace.Value.Start < bookingCheck.Value.Start && bookingWorkspace.Value.EndPreperations < bookingCheck.Value.EndPreperations)
                        {
                            isOverlappingCheck = true;
                        }
                    }

                    if(isOverlappingCheck)
                    {
                        _bookings.Add(bookingWorkspace.Key, bookingCopy);
                        return StatusCode(StatusCodes.Status400BadRequest, "Unable to complete due overlapping booking dates");
                    }

                    bookingCopy.Units = bookingWorkspace.Value.Units;
                    bookingCopy.PreparationTimeInDays = bookingWorkspace.Value.PreparationTimeInDays;
                    _bookings.Add(bookingWorkspace.Key, bookingCopy);
                }

                return Ok(new ResourceIdViewModel { Id = model.Id });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
