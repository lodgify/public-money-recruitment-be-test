using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Booking
{
    public class BookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingViewModel>
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public Task<BookingViewModel> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = _bookingRepository.Get(new BookingId(request.Id));
            return null;
        }
    }
}
