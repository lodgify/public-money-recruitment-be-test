using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public CalendarController(IRepository<Rental> rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public async Task<CalendarDto> Get(int rentalId, DateTime start, int nights)
        {
            var query = new GetRentalCalendarQuery(rentalId, start, nights);
            var handler = new GetRentalCalendarQueryHandler(_rentalRepository, _bookingRepository);
            return handler.Handle(query);
        }
    }
}
