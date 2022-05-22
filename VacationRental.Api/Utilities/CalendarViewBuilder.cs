using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Api.Utilities
{
    public class CalendarViewBuilder
    {
        public CalendarViewModel CalendarView(CalendarDto model, List<List<KeyValuePair<int, Booking>>> bookings, int preparationTime)
        {
            var result = new CalendarViewModel
            {
                RentalId = model.rentalId,
                Dates = new List<CalendarDateViewModel>(),
            };

            for (int i = 0; i < bookings.Count; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = model.start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                };

                foreach (var booking in bookings[i])
                {
                    if (booking.Value.RentalId == model.rentalId
                        && booking.Value.Start <= date.Date && booking.Value.Start.AddDays(booking.Value.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Value.Id, Unit = booking.Value.Unit });
                        date.PreparationTimes.Add(new CalendarPreparationTimeViewModel()
                        {
                            PreparationTimeInDays = preparationTime,
                            Unit = booking.Value.Unit

                        });
                    }
                }
                result.Dates.Add(date);
            }
            return result;
        }
    }
}
