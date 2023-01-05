using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Domain.Messages.Bookings;

namespace VacationRental.Application.Features.Bookings.Queries.GetBooking
{
    public sealed record class GetBookingQuery(int bookingId) : IQuery<BookingDto>
    {
    }
}
