using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessObjects;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;
        private readonly IMapper _mapper;

        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IBookingsService bookingsService,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _bookingsService = bookingsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            var bookingEntity = _bookingsService.GetBooking(bookingId);
            var bookingViewModel = _mapper.Map<BookingViewModel>(bookingEntity);

            return bookingViewModel;
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            var createBooking = _mapper.Map<CreateBooking>(model);
            var bookingId = _bookingsService.CreateBooking(createBooking);

            var key = new ResourceIdViewModel { Id = bookingId };

            return key;
        }
    }
}
