using MediatR;

namespace VacationRental.Application.Bookings.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<BookingViewModel>
    {
        public int BookingId { get; set; }
    }
}