using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Domain.Models;
using VacationRental.Services.IServices;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {

            return _bookingService.Get(bookingId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {

            

            return _bookingService.Create(model);
        }

    }
}

