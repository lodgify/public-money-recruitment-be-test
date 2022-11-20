using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Application.DTO;
using VacationRental.Core.Entities;

namespace VacationRental.Application.Queries.Handlers
{
    internal static class Extensions
    {
        public static RentalDto AsDto(this Rental rental) => new()
        {
            Id = rental.Id,
            Units = rental.Units
        };

        public static BookingDto AsDto(this Booking booking) => new()
        {
            Id = booking.Id,
            Nights = booking.Nights,
            RentalId = booking.RentalId,
            Start = booking.Start
        };

        public static CalendarDto AsDto(this Rental rental, DateTime start, int nights)
        {
            var result = new CalendarDto
            {
                RentalId = rental.Id,
                Dates = GetCalendarDateDtos().ToList()
            };

            IEnumerable<CalendarDateDto> GetCalendarDateDtos()
            {
                for (var i = 0; i < nights; i++)
                {
                    var date = new CalendarDateDto
                    {
                        Date = start.Date.AddDays(i),
                        Bookings = new List<CalendarBookingDto>(),
                        PreparationTimes = new List<CalendarPreparationTimeDto>()
                    };

                    foreach (var booking in rental.Bookings)
                    {
                        var status = booking.GetStatus(date.Date);

                        if (status is BookingStatus.Booked)
                            date.Bookings.Add(new CalendarBookingDto { Id = booking.Id, Unit = booking.Unit });
                        else if (status is BookingStatus.Preparation)
                            date.PreparationTimes.Add(new CalendarPreparationTimeDto { Unit = booking.Unit });
                    }

                    yield return date;
                }
            }

            return result;
        }
    }
}
