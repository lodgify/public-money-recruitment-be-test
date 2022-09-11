using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Helpers;
using VacationRental.Common.Models;
using VacationRental.Service.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookService;

        public BookingsController(IRentalService rentalService, IBookingService bookingService)
        {
            _rentalService = rentalService;
            _bookService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            var booking = _bookService.Get(bookingId);
            if (booking is null)
                throw new ApplicationException("Booking not found");

            return booking;
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            var rental = _rentalService.Get(model.RentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            var item = new BookingViewModel
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            };
            var bookings = _bookService.GetAll();
            if (!BookingHelper.CheckAvailability(item, bookings, rental))
            {
                throw new ApplicationException("Not available");
            }

            item = _bookService.AddOrUpdate(item);

            var key = new ResourceIdViewModel { Id = item.Id };
            return key;
        }
    }
}
