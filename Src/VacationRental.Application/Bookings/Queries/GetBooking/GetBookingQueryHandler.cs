using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VacationRental.Application.Bookings.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingViewModel>
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public GetBookingQueryHandler(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
        }

        public async Task<BookingViewModel> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            if (!_bookings.ContainsKey(request.BookingId))
                return null;
    
            return await Task.FromResult(_bookings[request.BookingId]);
        }
    }
}