using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Validation;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IBookingService _service;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IBookingService service)
        {
            _rentals = rentals;
            _bookings = bookings;
            _service = service;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            _service.bookingGetValidation(bookingId,_bookings);
            return _bookings[bookingId];
        }


        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            ResourceIdViewModel key = _service.addResourceIdViewModel(model,_rentals,_bookings);
            return key;
        }
    }
}
