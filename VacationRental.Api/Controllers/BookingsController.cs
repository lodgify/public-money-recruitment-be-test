using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Models;
using VacationRental.Models.Booking;
using VacationRental.Models.Rental;

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
        public BookingViewModel Get(int bookingId)
        {
            try
            {
                BookingViewModel result = Services.Bookings.getBookingById(bookingId);
                if (result.Id == null)
                    throw new ApplicationException("Booking not found");
                else return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            try
            {
                return Services.Bookings.insert(model);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }                
        }
    }
}
