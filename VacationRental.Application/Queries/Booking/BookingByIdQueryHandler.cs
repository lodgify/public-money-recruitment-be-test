using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Repositories.ReadOnly;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Booking
{
    public class BookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingViewModel>
    {
        private readonly IBookingReadOnlyRepository _bookingRepository;

        public BookingByIdQueryHandler(IBookingReadOnlyRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public async Task<BookingViewModel> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.Get(new BookingId(request.Id));
            return new BookingViewModel
            {
                Id = (int) booking.Id,
                Nights = booking.Period.Nights,
                Start = booking.Period.Start,
                RentalId = (int) booking.RentalId
            };
        }
    }
}
