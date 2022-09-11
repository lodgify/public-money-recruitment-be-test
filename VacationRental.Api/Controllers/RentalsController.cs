using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Helpers;
using VacationRental.Common.Models;
using VacationRental.Service.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookService;


        public RentalsController(IRentalService rentalService, IBookingService bookingService)
        {
            _rentalService = rentalService;
            _bookService = bookingService;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            var rental = _rentalService.Get(rentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            var bookings = _bookService.GetAll();

            if (rental.Units != model.Units ||
                rental.PreparationTimeInDays != model.PreparationTimeInDays && BookingHelper.CheckAvailability(bookings, rental))
            {
                rental.Units = model.Units;
                rental.PreparationTimeInDays = model.PreparationTimeInDays;
                _rentalService.AddOrUpdate(rental);
            }
            
            return rental;
        }
        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalService.Get(rentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {

            var item = _rentalService.AddOrUpdate(new RentalViewModel
            {
                PreparationTimeInDays = model.PreparationTimeInDays,
                Units = model.Units
            });

            var key = new ResourceIdViewModel { Id = item.Id };

            return key;
        }
    }
}
