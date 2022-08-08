using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarService(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ApplicationException("Nights must be positive");

            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var bookings = _bookings.Values.Where(x => x.RentalId == rentalId);

            var result = GenerateCalendar(rentalId, start, nights, bookings);

            return result;
        }

        private CalendarViewModel GenerateCalendar(int rentalId, DateTime start, int nights, IEnumerable<BookingViewModel> bookings)
        {
            var calendar = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var preparationDays = _rentals.Values.FirstOrDefault(p => p.Id == rentalId).PreparationTimeInDays;

            for (var i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var calendarBookings = GetCalendarBookings(bookings, date);
                var calendarPreparationTimes = GetCalendarPreparationTimes(bookings, date, preparationDays);

                calendar.Dates.Add(new CalendarDateViewModel
                {
                    Date = date,
                    Bookings = calendarBookings,
                    PreparationTimes = calendarPreparationTimes
                });
            }

            return calendar;
        }

        private List<CalendarBookingViewModel> GetCalendarBookings(IEnumerable<BookingViewModel> bookings, DateTime date)
        {
            return bookings
                    .Where(
                        p => p.Start <= date.Date &&
                        p.End > date.Date)
                    .Select(p => new CalendarBookingViewModel
                    {
                        Id = p.Id,
                        Unit = p.Unit,
                    }).ToList();
        }
        private List<CalendarPreparationTimeViewModel> GetCalendarPreparationTimes(IEnumerable<BookingViewModel> bookings, DateTime date, int preparationDays)
        {
            return bookings
                    .Where(
                        p => p.End <= date.Date &&
                        p.End.AddDays(preparationDays) > date.Date)
                    .Select(p => new CalendarPreparationTimeViewModel
                    {
                        Unit = p.Unit,
                    }).ToList();
        }
    }
}
