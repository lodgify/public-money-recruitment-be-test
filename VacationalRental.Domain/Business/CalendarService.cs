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
        private readonly IRentalsRepository _rentalsRepository;
        public CalendarService(
            IBookingsRepository bookingRepository,
            IRentalsRepository rentalsRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalsRepository = rentalsRepository;
        }

        public async Task<CalendarModel> GetRentalCalendarByNights(int rentalId, DateTime start, int nights)
        {
            var result = new CalendarModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateModel>()
            };

            var bookings = _bookingRepository.GetBookinByRentalId(rentalId);
            var rentalPreparationTimeInDays = _rentalsRepository.GetRentalPreparationTimeInDays(rentalId);

            for (var night = 0; night < nights; night++)
            {
                var date = new CalendarDateModel
                {
                    Date = start.Date.AddDays(night),
                    Bookings = new List<CalendarBookingModel>(),
                    PreparationTimes = new List<PreparationTimesModel>()
                };

                foreach (var booking in await bookings)
                {
                    if (booking.Start <= date.Date && 
                        booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingModel { Id = booking.Id, Unit = booking.Unit });
                    }
                    else if (booking.Start <= date.Date &&
                             booking.Start.AddDays(booking.Nights + await rentalPreparationTimeInDays) > date.Date)
                    {
                        date.Date = booking.Start.AddDays(night);
                        date.PreparationTimes.Add(new PreparationTimesModel { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
