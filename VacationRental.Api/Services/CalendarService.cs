using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public CalendarService(
            IRentalRepository rentalRepository,
            IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ApplicationException("Nights must be positive");

            if (!_rentalRepository.HasValue(rentalId))
                throw new ApplicationException("Rental not found");

            var bookings = _bookingRepository.GetBookingsByRentalId(rentalId);

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

            var preparationDays = _rentalRepository.Get(rentalId).PreparationTimeInDays;

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
            var result = bookings
                .Where(
                    p => p.Start <= date.Date &&
                         p.End > date.Date)
                .Select(p => new CalendarBookingViewModel
                {
                    Id = p.Id,
                    Unit = p.Unit,
                }).ToList();
            return result;
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
