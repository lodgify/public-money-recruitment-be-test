using System.Linq;
using VacationRental.Application.DTO;

namespace VacationRental.Api.Models
{
    internal static class Extensions
    {
        public static RentalViewModel AsViewModel(this RentalDto rentalDto) => new()
        {
            Id = rentalDto.Id,
            Units = rentalDto.Units
        };

        public static BookingViewModel AsViewModel(this BookingDto bookingDto) => new()
        {
            Id = bookingDto.Id,
            Nights = bookingDto.Nights,
            RentalId = bookingDto.RentalId,
            Start = bookingDto.Start
        };

        public static CalendarViewModel AsViewModel(this CalendarDto calendarDto) => new()
        {
            RentalId = calendarDto.RentalId,
            Dates = calendarDto.Dates.Select(d => new CalendarDateViewModel
            {
                Date = d.Date,
                Bookings = d.Bookings.Select(b => new CalendarBookingViewModel
                {
                    Id = b.Id,
                }).ToList()
            }).ToList()
        };
    }
}
