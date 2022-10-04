using MediatR;
using VacationRental.Application.Bookings.Models;

namespace VacationRental.Application.Bookings.Queries.GetBooking
{
    public class GetBookingsQuery : IRequest<BookingViewModel>
    {
        public int BookingId { get; set; }
    }
}
