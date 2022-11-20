using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Core.Exceptions;
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

        public async Task<BookingDto> HandleAsync(GetBooking query, CancellationToken cancellationToken = default)
        {
            var booking = await _bookingRepository.GetAsync(query.Id, cancellationToken);

            if (booking is null)
            {
                throw new BookingNotExistException(query.Id);
            }

            return booking.AsDto();
        }
    }
}
