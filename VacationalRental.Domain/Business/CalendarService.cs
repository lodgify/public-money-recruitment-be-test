using System;
using System.Linq;
using System.Collections.Generic;
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
            GetRentalCalendarByNightsValidations(rentalId, start, nights);

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
                    var minValueDate = booking.Start;
                    var maxValueDate = booking.Start.AddDays(booking.Nights - 1);
                    var maxValueDateWithPrepDays = booking.Start.AddDays(booking.Nights + await rentalPreparationTimeInDays);

                    if (minValueDate <= date.Date &&
                        maxValueDate >= date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingModel { Id = booking.Id, Unit = booking.Unit });
                    }
                    else if (maxValueDate < date.Date && maxValueDate < maxValueDateWithPrepDays && date.Date < maxValueDateWithPrepDays)
                    {
                        var bookingsResult = await bookings;
                        var minDate = bookingsResult.Min(a => a.Start)/*.AddDays(night)*/;

                        date.Date = minDate.AddDays(night);
                        date.PreparationTimes.Add(new PreparationTimesModel { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }

        private void GetRentalCalendarByNightsValidations(int rentalId, DateTime start, int nights)
        {
            if (rentalId == 0)
                throw new InvalidOperationException($"{nameof(rentalId)} can't be 0");

            if (start == DateTime.MinValue)
                throw new InvalidOperationException($"{nameof(start)} can't be {DateTime.MinValue}");

            if (nights == 0)
                throw new InvalidOperationException($"{nameof(nights)} can't be 0");
        }
    }
}
