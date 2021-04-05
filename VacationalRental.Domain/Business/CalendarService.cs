using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationalRental.Domain.Models;

namespace VacationalRental.Domain.Business
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingsRepository _bookingRepository;
        public CalendarService(IBookingsRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<CalendarModel> GetRentalCalendarByNights(int rentalId, DateTime start, int nights)
        {
            var result = new CalendarModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingModel>()
                };

                foreach (var booking in await _bookingRepository.GetBookings())
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
