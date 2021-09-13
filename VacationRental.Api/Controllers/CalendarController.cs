using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _iCalendarService;

        public CalendarController(ICalendarService iCalendarService)
        {
            _iCalendarService = iCalendarService;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime bookingStartDate, int numberOfnights)
        {             
            GetCalendarResponse response = _iCalendarService.GetBooking(new GetCalendarRequest() {rentalId = rentalId,bookingStartDate = bookingStartDate, numberOfnights= numberOfnights });

            if (response.Success)
            {
                return response.CalendarViewModel;
            }
            else
            {
                // To don't change the signature (I like to return a Json with the error)
                throw new Exception(response.Message);
            }
        }
    }
}
