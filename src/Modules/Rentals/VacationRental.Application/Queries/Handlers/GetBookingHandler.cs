using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Application.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries.Handlers
{
    internal class GetBookingHandler : IQueryHandler<GetBooking, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public Task<BookingDto> HandleAsync(GetBooking query, CancellationToken cancellationToken = default)
        {
            var booking = _bookingRepository.Get(query.Id);

            if (booking is null)
            {
                throw new BookingNotFoundException(query.Id);
            }

            return Task.FromResult(booking.AsDto());
        }
    }
}
