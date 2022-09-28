using System;
using System.Collections.Generic;
using FluentValidation;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessLogic.Services.Models;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Services
{
    public class CalendarsService : ICalendarsService
    {
        private readonly IBookingsService _bookingsService;
        private readonly IRentalsService _rentalsService;
        private readonly IValidator<GetCalendarServiceModel> _getCalendarValidator;

        public CalendarsService(IBookingsService bookingsService,
            IRentalsService rentalsService,
            IValidator<GetCalendarServiceModel> getCalendarValidator)
        {
            _bookingsService = bookingsService;
            _rentalsService = rentalsService;
            _getCalendarValidator = getCalendarValidator;
        }

        public Calendar GetCalendar(int rentalId, DateTime start, int nights)
        {
            ValidateGetCalendarModel(rentalId, start, nights);

            var bookings = _bookingsService.GetBookings();
            var result = new Calendar
            {
                RentalId = rentalId,
                Dates = new List<CalendarDate>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDate
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBooking>()
                };

                foreach (var booking in bookings)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBooking { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }

        private void ValidateGetCalendarModel(int rentalId, DateTime start, int nights)
        {
            var getCalendarServiceModel = new GetCalendarServiceModel
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };
            _getCalendarValidator.ValidateAndThrow(getCalendarServiceModel);

            var rental = _rentalsService.GetRental(rentalId);
            if (rental == null)
            {
                throw new ArgumentException($"Rental with id {rentalId} not found.");
            }
        }
    }
}
