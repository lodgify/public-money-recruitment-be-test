using MediatR;

namespace VR.Application.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<GetBookingResponse>
    {
        public GetBookingQuery(int bookingId)
        {
            BookingId = bookingId;
        }

        public int BookingId { get; set; }
    }
}
