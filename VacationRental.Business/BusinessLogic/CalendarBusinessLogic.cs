using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Models;
using VacationRental.Business.Validators;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Business.BusinessLogic
{
    public class CalendarBusinessLogic
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingsRepository _bookingsRepository;

        public CalendarBusinessLogic(IRentalRepository rentalRepository, IBookingsRepository bookingsRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingsRepository = bookingsRepository;
        }

        public CalendarViewModel GetRentalCalendar(int rentalId, DateTime start, int nights)
        {
            if (!IsPositiveNumberValidator.Validate(nights))
                throw new ApplicationException("Nigts must be positive");
            if (!_rentalRepository.Exists(rentalId))
                throw new ApplicationException("Rental not found");

            DateTime end = start.AddDays(nights);
            var rental = _rentalRepository.Get(rentalId);
            var rentalBookings = _bookingsRepository.GetAll(booking =>
                    booking.RentalId == rentalId
                    && booking.Start >= start || booking.Start.AddDays(booking.Nights) <= end)
                .ToList();

            var calendar = InitCalendar(start, nights);
            calendar = FillCalendar(calendar, rentalBookings, start, end, rental.PreparationTimeInDays);

            return new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = calendar
            };
        }

        private List<CalendarDateViewModel> InitCalendar(DateTime start, int nights)
        {
            List<CalendarDateViewModel> dates = new List<CalendarDateViewModel>();

            for (var i = 0; i < nights; i++)
                dates.Add(new CalendarDateViewModel
                {
                    Date = start.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                });

            return dates;
        }

        private List<CalendarDateViewModel> FillCalendar(List<CalendarDateViewModel> calendar, List<Booking> rentalBookings, DateTime start, DateTime end, int preparationTime)
        {
            foreach (var booking in rentalBookings)
            {
                DateTime bookingEnd = booking.Start.AddDays(booking.Nights);
                DateTime calendarBookingStart = booking.Start < start ? start : booking.Start;
                DateTime calendarBookingEnd = bookingEnd > end ? end : bookingEnd;
                DateTime prepEnd = calendarBookingEnd.AddDays(preparationTime) > end ? end : calendarBookingEnd.AddDays(preparationTime);

                int choosenUnit = SelectUnit(calendar, prepEnd, calendarBookingStart);

                var calendarBooking = new CalendarBookingViewModel
                {
                    Id = booking.Id,
                    Unit = choosenUnit,
                };

                var bookingRangeCalendarDates = calendar.Where(date => calendarBookingStart <= date.Date && bookingEnd > date.Date);

                foreach (var bookingRangeCalendarDate in bookingRangeCalendarDates)
                    bookingRangeCalendarDate.Bookings.Add(calendarBooking);


                var bookingPreparationTime = new PreparationTimeViewModel
                {
                    Unit = choosenUnit,
                };

                var preparationRangeCalendarDates = calendar.Where(date => bookingEnd < date.Date && prepEnd >= date.Date);

                foreach (var preparationRangeCalendarDate in preparationRangeCalendarDates)
                    preparationRangeCalendarDate.PreparationTimes.Add(bookingPreparationTime);

            }

            return calendar;
        }

        private int SelectUnit(List<CalendarDateViewModel> calendar, DateTime prepEnd, DateTime calendarBookingStart)
        {
            var designatedPeriodCalendarBookings = calendar.Where(date => calendarBookingStart <= date.Date && prepEnd >= date.Date).SelectMany(date => date.Bookings);
            var preparationTimes = calendar.Where(date => calendarBookingStart <= date.Date && prepEnd >= date.Date).SelectMany(date => date.PreparationTimes);

            var unavailableUnits = designatedPeriodCalendarBookings.Select(cBooking => cBooking.Unit).ToList();
            unavailableUnits.AddRange(preparationTimes.Select(time => time.Unit));
            unavailableUnits = unavailableUnits.Distinct().OrderBy(unit => unit).ToList();

            int choosenUnit = 1;
            for (int i = 0; i < unavailableUnits.Count; i++)
            {
                if (i + 1 != unavailableUnits[i])
                {
                    choosenUnit = i + 1;
                    break;
                }

                choosenUnit = unavailableUnits[i] + 1;
            }

            return choosenUnit;
        }

    }
}
