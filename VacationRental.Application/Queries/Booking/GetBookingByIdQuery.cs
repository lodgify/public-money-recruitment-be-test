using MediatR;

namespace VacationRental.Application.Queries.Booking
{
    public sealed class GetBookingByIdQuery : IRequest<BookingViewModel>
    {
        public GetBookingByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
