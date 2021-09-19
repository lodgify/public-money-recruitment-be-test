using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Library;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public IActionResult Get(int bookingId)
        {
            try
            {
                if(!_bookings.ContainsKey(bookingId))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Booking not found");
                }
                return Ok(_bookings[bookingId]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(BookingBindingModel model)
        {
            try
            {   
                if (!_rentals.ContainsKey(model.RentalId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Rental not found");
                }
                               
                var rental = _rentals[model.RentalId];
                var existingUnitsInDatetimeThreshold = this.GetUsedUnitsInRental(model, rental);
                var avaibleUnitsByBooking = rental.Units - existingUnitsInDatetimeThreshold;

                if (rental.Units < model.Units || avaibleUnitsByBooking < model.Units)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Not available");
                }

                ResourceIdViewModel vm = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
                _bookings.Add(vm.Id, new BookingViewModel
                {
                    Id = vm.Id,
                    Nights = model.Nights,
                    RentalId = model.RentalId,
                    Start = model.Start.Date,
                    Units = model.Units,
                    PreparationTimeInDays = rental.PreparationTimeInDays
                });
                return Ok(vm);                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(BookingBindingModel model)
        {
            try
            {
                if (!_rentals.ContainsKey(model.RentalId) || !_bookings.ContainsKey(model.BookingId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Rental or booking not found");
                }

                var rental = _rentals[model.RentalId];
                var booking = _bookings.First(b => b.Key == model.BookingId);

                _bookings.Remove(booking.Key);
                var units = this.GetUsedUnitsInRental(model, rental);
                var avaibleUnitsByBooking = rental.Units - units;

                if (rental.Units < model.Units || avaibleUnitsByBooking < model.Units)
                {
                    _bookings.Add(booking);
                    return StatusCode(StatusCodes.Status400BadRequest, "Not available");
                }

                if (booking.Value.RentalId == model.RentalId)                
                {
                    booking.Value.Nights = model.Nights;
                    booking.Value.Start = model.Start.Date;
                    booking.Value.Units = model.Units;
                    _bookings.Add(booking);   
                }
                else
                {
                    var id = _bookings.Where(b => b.Value.RentalId == model.RentalId).Count();
                    ResourceIdViewModel vm = new ResourceIdViewModel { Id = id };
                    _bookings.Add(vm.Id, new BookingViewModel
                    {
                        Id = vm.Id,
                        Nights = model.Nights,
                        RentalId = model.RentalId,
                        Start = model.Start.Date,
                        Units = model.Units,
                        PreparationTimeInDays = rental.PreparationTimeInDays
                    });
                }

                return Ok(new ResourceIdViewModel { Id = booking.Key });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private int GetUsedUnitsInRental(BookingBindingModel model, RentalViewModel rental)
        {
            return
                _bookings
                    .Where(b => b.Value.RentalId == model.RentalId 
                            && (b.Value.Start, b.Value.EndPreperations).IsBewteenTwoDates(model.Start, model.End.AddDays(rental.PreparationTimeInDays)))
                    .Sum(b => b.Value.Units);
        }
    }
}
