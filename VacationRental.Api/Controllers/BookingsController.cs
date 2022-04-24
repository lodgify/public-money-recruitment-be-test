using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController( IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public ActionResult<BookingDTO> Get(int bookingId)
        {
            var booking = _bookingService.GetBooking(bookingId);

            var result = booking.Adapt<BookingDTO>();
            return result;
        }

        [HttpPost]
        public ActionResult<BookingsCreateOutputDTO> Post(BookingsCreateInputDTO model)
        {
            var id = _bookingService.CreateBooking(model);

            var result = id.Adapt<BookingsCreateOutputDTO>();

            return result;
        }
    }
}
