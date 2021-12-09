using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace RentalSoftware.API.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        //About to remove asynchronous calls
        [HttpGet("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            var response = await _bookingService.GetBooking(new GetBookingRequest() 
            { 
                BookingId = bookingId 
            });

            if (!response.Succeeded)
            {
                throw new Exception(response.Message);
            }

            return response.BookingViewModel;
        }

        [HttpPost]
        public async Task<IdentifierViewModel> Post(BookingViewModel model)
        {
            var response = await _bookingService.AddBooking(new AddBookingRequest() 
            {
                Nights = model.Nights, 
                RentalId = model.RentalId, 
                StartDate = model.Start 
            });

            if (!response.Succeeded)
            {
                throw new ApplicationException(response.Message);
            }

            return response.ResourceIdViewModel;

        }
    }
}
