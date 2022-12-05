using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Business.BusinessLogic;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingBusinessLogic _bookingBusinessLogic;
        public BookingsController(BookingBusinessLogic bookingBusinessLogic)
        {
            _bookingBusinessLogic = bookingBusinessLogic;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            return _bookingBusinessLogic.GetBooking(bookingId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            var booking = _bookingBusinessLogic.AddBooking(model);
            var key = new ResourceIdViewModel { Id = booking.Id };

            return key;
        }
    }
}
