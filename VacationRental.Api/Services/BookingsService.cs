using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public class BookingsService : GeneralService<BookingViewModel>, IBookingsService
    {
        public BookingsService(IDictionary<int, BookingViewModel> dictionary) : base(dictionary)
        {
        }

        protected virtual int GetBookingsUnavailableCount(BookingBindingModel model, RentalViewModel rental)
        {
            var bookings = GetBookingsByRentalId(model.RentalId);

            return bookings.Values.Where(booking =>
            {
                var bookedDays = booking.Nights + rental.PreparationTimeInDays;
                var days = model.Nights + rental.PreparationTimeInDays;

                return booking.RentalId == model.RentalId
                    && (booking.Start <= model.Start.Date && booking.Start.AddDays(bookedDays) > model.Start.Date)
                    || (booking.Start < model.Start.AddDays(days) && booking.Start.AddDays(bookedDays) >= model.Start.AddDays(days))
                    || (booking.Start > model.Start && booking.Start.AddDays(bookedDays) < model.Start.AddDays(days));
            }).Count();
        }

        public virtual IDictionary<int, BookingViewModel> GetBookingsByRentalId(int rentalId)
            => Dictionary.Where(x => x.Value.RentalId == rentalId).ToDictionary(x => x.Key, x => x.Value);

        public bool CheckIfBookingIsAvailable(BookingBindingModel model, RentalViewModel rental)
            => GetBookingsUnavailableCount(model, rental) < rental.Units;

        public virtual ResourceIdViewModel AddBooking(BookingBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = GenerateId() };

            Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return key;
        }

        public virtual CalendarViewModel GetBookingCalendar(RentalViewModel rental, DateTime start, int nights)
        {
            var result = new CalendarViewModel
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < (nights + rental.PreparationTimeInDays); i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                date.Bookings = Dictionary.Where(booking =>
                {
                    var bookedDays = booking.Value.Nights + rental.PreparationTimeInDays;

                    return booking.Value.RentalId == rental.Id
                        && booking.Value.Start <= date.Date && booking.Value.Start.AddDays(bookedDays) > date.Date;
                }).Select(booking => new CalendarBookingViewModel { Id = booking.Value.Id }).ToList();

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
