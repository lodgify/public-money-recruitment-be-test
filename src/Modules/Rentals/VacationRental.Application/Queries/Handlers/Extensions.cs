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
    }
}
