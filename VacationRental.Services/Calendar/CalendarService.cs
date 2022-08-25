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

        public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
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

            var bookings = _bookingRepository
                .GetAll()
                .ToList();

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
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in bookings)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
