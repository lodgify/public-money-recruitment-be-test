using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Dtos;
using VacationRental.Application.Midlewares.Booking;
using VacationRental.Application.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingMiddleware _bookingMiddleware;

        public BookingsController(IBookingMiddleware bookingMiddleware)
        {
            _bookingMiddleware = bookingMiddleware;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModelOutput> Get(int bookingId)
        {
            var response = await this._bookingMiddleware.GetBookingById(bookingId);
            return response;
        }

        [HttpPost]
        public async Task<int> Post(BookingBindingModel model)
        {
            var booking = new BookingInputDto(model.RentalId, model.Start, model.Nights);
            var response = await this._bookingMiddleware.CreateBooking(booking);
            return response;
        }
    }
}
