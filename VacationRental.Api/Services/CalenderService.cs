using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Validation;

namespace VacationRental.Api.Services
{
    public class CalenderService:ICalenderService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarViewModel CalenderGetService(int rentalId, DateTime start, int nights,
            IDictionary<int, RentalViewModel> _rentals, IDictionary<int, BookingViewModel> _bookings)
        {
            validationGetService(rentalId, start, nights, _rentals, _bookings);
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
                    PreparationTimes = new List<CalendarPreparationViewModel>(),
                };
                addBookingToList(rentalId, _bookings, date);
                addPreparationToList(rentalId, _rentals, _bookings, date);
                result.Dates.Add(date);
            }

            return result;
        }

        private static void validationGetService(int rentalId, DateTime start, int nights, IDictionary<int, RentalViewModel> _rentals, IDictionary<int, BookingViewModel> _bookings)
        {
            var positiveNight = new PositiveNightHandler();
            var rentalExist = new RentalExistHandler();
            positiveNight.SetNext(rentalExist);
            var model = new BookingBindingModel();
            model.Nights = nights;
            model.Start = start;
            model.RentalId = rentalId;
            positiveNight.Handle(model, _rentals, _bookings);
        }
        private static void addBookingToList(int rentalId, IDictionary<int, BookingViewModel> _bookings, CalendarDateViewModel date)
        {
            var bookingDays = _bookings.Values.Where(b => b.RentalId == rentalId &&
            b.Start <= date.Date &&
            b.Start.AddDays(b.Nights) > date.Date).ToList();
            bookingDays.ForEach(v =>
            {
                date.Bookings.Add(new CalendarBookingViewModel { Id = v.Id, Unit = bookingDays.Count() });
            });
        }
        private static void addPreparationToList(int rentalId, IDictionary<int, RentalViewModel> _rentals, IDictionary<int, BookingViewModel> _bookings, CalendarDateViewModel date)
        {
            var preparationInRental = _rentals.Values.Where(r => r.Id == rentalId).First().PreparationTimeInDays;
            var preparationDay = _bookings.Values.Where(p => p.RentalId == rentalId &&
            p.Start.AddDays(p.Nights) <= date.Date &&
            p.Start.AddDays(p.Nights + preparationInRental) > date.Date).ToList();
            preparationDay.ForEach(p =>
            {
                date.PreparationTimes.Add(new CalendarPreparationViewModel { Id=p.Id,Unit = preparationDay.Count() });
            });
        }

    }
}
