﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<BookingViewModel> Get(int bookingId)
        {
            try
            {
                if (!_bookings.ContainsKey(bookingId))
                    return NotFound("Booking not found");
                //throw new ApplicationException();

                return _bookings[bookingId];
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                if (model.Nights <= 0)
                    return BadRequest("Nigts must be positive");
                //throw new ApplicationException();
                if (!_rentals.ContainsKey(model.RentalId))
                    return NotFound("Rental not found");
                //throw new ApplicationException();

                for (var i = 0; i < model.Nights; i++)
                {
                    var count = 0;
                    foreach (var booking in _bookings.Values)
                    {
                        if (booking.RentalId == model.RentalId
                            && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                            || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                            || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                        {
                            count++;
                        }
                    }
                    if (count >= _rentals[model.RentalId].Units)
                        return Ok("Not available");
                    //throw new ApplicationException();
                }


                var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

                _bookings.Add(key.Id, new BookingViewModel
                {
                    Id = key.Id,
                    Nights = model.Nights,
                    RentalId = model.RentalId,
                    Start = model.Start.Date
                });

                return key;
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
