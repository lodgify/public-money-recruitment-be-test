using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        private readonly IBookingsService _bookingsService;

        public BookingsController(
            IRentalsService rentalsService,
            IBookingsService bookingsService)
        {
            _rentalsService = rentalsService;
            _bookingsService = bookingsService;
        }

        [HttpGet("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            var booking = _bookingsService.Get(bookingId);
            if (booking == null)
                throw new ApplicationException("Booking not found");

            return booking;
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            var rental = _rentalsService.Get(model.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var isAvailable = _bookingsService.CheckIfBookingIsAvailable(model, rental);
            if (!isAvailable)
                throw new ApplicationException("Not available");

            return _bookingsService.AddBooking(model);
        }
    }
}
