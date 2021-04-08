using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRentalsRepository _rentalsRepository;
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(
            IRentalsRepository rentalsRepository,
            ICalendarService calendarService,
            ILogger<CalendarController> logger)
        {
            _rentalsRepository = rentalsRepository;
            _calendarService = calendarService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            try
            {
                if (rentalId <= 0)
                    return BadRequest("Rental can't be with id 0");

                if (nights <= 0)
                    return BadRequest("Nights must be positive");

                if (!await _rentalsRepository.RentalExists(rentalId))
                    return NotFound("Rental not found");

                var calendarModel = await _calendarService.GetRentalCalendarByNights(rentalId, start, nights);

                return new CalendarViewModel
                {
                    RentalId = calendarModel.RentalId,
                    Dates = calendarModel.Dates.Select(a => 
                    new CalendarDateViewModel 
                    { 
                        Bookings = a.Bookings.Select(b => new CalendarBookingViewModel { Id = b.Id, Unit = b.Unit }).ToList(),
                        Date = a.Date,
                        PreparationTime = a.PreparationTimes.Select(b => new PreparationTimesViewModel { Unit = b.Unit }).ToList()
                    }).ToList(),
                };
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, nameof(Get));

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
