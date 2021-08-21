using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IDictionary<int, IList<PreparationTimeModel>> _preparationTimes;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IDictionary<int, IList<PreparationTimeModel>> preparationTimes)
        {
            _rentals = rentals;
            _bookings = bookings;
            _preparationTimes = preparationTimes;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeModel>()
                };

                foreach (var booking in _bookings.Values.Where(b => b.RentalId == rentalId))
                {
                    if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                    }
                    else
                    {
                        if (_preparationTimes.TryGetValue(booking.RentalId, out IList<PreparationTimeModel> preparationModelList))
                        {
                            foreach (var pm in preparationModelList)
                            {
                                if (booking.Unit == pm.Unit && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights + _rentals[rentalId].PreparationTimeInDays) > date.Date)
                                {
                                    date.PreparationTimes.Add(pm);
                                }
                            }
                        }
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
