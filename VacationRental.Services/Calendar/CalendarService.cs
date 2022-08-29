using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Data;
using VacationRental.Core.Domain.Bookings;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Exceptions;
using VacationRental.Services.Models.Calendar;

namespace VacationRental.Services.Calendar
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository<BookingEntity, int> _bookingRepository;
        private readonly IRepository<RentalEntity, int> _rentalRepository;

        public CalendarService(
            IRepository<BookingEntity, int> bookingRepository,
            IRepository<RentalEntity, int> rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public CalendarDto GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
            {
                throw new ApplicationException("Nights must be positive");
            }

            var rental = _rentalRepository.GetById(rentalId);
            if (rental == null)
            {
                throw new RentalNotFoundException("Rental not found");
            }

            var result = GenerateCalendar(rental, start, nights);

            return result;
        }

        private CalendarDto GenerateCalendar(RentalEntity rental, DateTime start, int nights)
        {
            var calendar = new CalendarDto
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDateDto>()
            };

            var preparationTime = rental.PreparationTime;
            var bookings = GetBookingsByRentalId(rental.Id);

            for (var i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var calendarBookings = GetCalendarBookings(bookings, date);
                var calendarPreparationTimes = GetCalendarPreparationTimes(bookings, date, preparationTime);

                calendar.Dates.Add(new CalendarDateDto
                {
                    Date = date,
                    Bookings = calendarBookings,
                    PreparationTimes = calendarPreparationTimes
                });
            }

            return calendar;
        }

        private List<CalendarPreparationTimeDto> GetCalendarPreparationTimes(List<BookingEntity> bookings, DateTime date, int preparationTime)
        {
            return bookings
                .Where(x => x.End <= date.Date && x.End.AddDays(preparationTime) > date.Date)
                .Select(x => new CalendarPreparationTimeDto
                {
                    Unit = x.Unit,
                })
                .ToList();
        }

        private List<CalendarBookingDto> GetCalendarBookings(List<BookingEntity> bookingQuery, DateTime date)
        {
            var result = bookingQuery
                .Where(x => x.Start <= date.Date && x.Start.AddDays(x.Nights) > date.Date)
                .Select(x => new CalendarBookingDto
                {
                    Id = x.Id,
                    Unit = x.Unit,
                })
                .ToList();

            return result;
        }

        private List<BookingEntity> GetBookingsByRentalId(int rentalId)
        {
            var query = _bookingRepository.Table
                .Where(x => x.RentalId == rentalId);

            return query.ToList();
        }
    }
}
