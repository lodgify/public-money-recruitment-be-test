using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Utilities;
using VacationRental.Business.Abstract;
using VacationRental.Business.Mapper;
using VacationRental.Domain.DTOs;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly IRentalService rentalService;
        public BookingsController(IBookingService _bookingService,
                                  IRentalService _rentalService)
        {
            bookingService = _bookingService;
            rentalService = _rentalService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public IActionResult Get(int bookingId)
        {
            var booking = bookingService.GetById(bookingId);

            if (booking is null)
            {
                return NotFound("Booking not found");
            }

            var rental = rentalService.GetById(booking.RentalId);

            return Ok(booking.ToBl(rental));
        }

        [HttpPost]
        public IActionResult Post(BookingDto model)
        {
            var rental = rentalService.GetById(model.RentalId);

            if (rental is null)
            {
                return NotFound("Rental not found");
            }

            var bookings = bookingService.GetAll(model.RentalId);

            bookings.StartDates.Add(model.Start);
            bookings.EndDates.Add(model.Start.AddDays(model.Nights));

            int unitNumber = BookingHandler.CheckBookings(
                               bookings.StartDates.ToList(),
                               bookings.EndDates.ToList(),
                               rentalService.GetById(model.RentalId).Units
                               );

            if (unitNumber > rental.Units)
            {
                return BadRequest("Not available");
            }

            model.Unit = unitNumber;

            bookingService.Create(model.ToDb(rental));
            return Ok(model);
        }
    }
}
