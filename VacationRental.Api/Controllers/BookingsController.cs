using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _iBookingService;
        
        public BookingsController(IBookingService iBookingService)
        {
            _iBookingService = iBookingService;            
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            GetBookingResponse response = _iBookingService.GetBooking(new GetBookingRequest() { bookingId = bookingId });

            if (response.Success)
            {
                return response.BookingViewModel;
            }
            else
            {
                // To don't change the signature (I like to return a Json with the error)
                throw new Exception(response.Message);
            }
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            AddBookingResponse response = _iBookingService.AddBooking(new AddBookingRequest() { NumberOfNigths = model.Nights, RentalId = model.RentalId, StartDate = model.Start });

            if (response.Success)
            {
                return response.ResourceIdViewModel;
            }
            else
            {
                // To don't change the signature (I like to return a Json with the error)
                return response.ResourceIdViewModel;
            }
        }
    }
}
