using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Queries.GetBooking
{
    public class GetBookingQueryHandler : BaseRequestHandler<GetBookingQuery, GetBookingResponse>
    {
        public GetBookingQueryHandler(IObjectMapper mapper, VRContext context) : base(mapper, context) { }

        public async override Task<GetBookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.FindAsync(request.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking is not found", $"GetBookingQuery - booking with id {request.BookingId} not found");

            return _mapper.Map<Booking, GetBookingResponse>(booking);
        }
    }
}
