using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries.Handlers
{
    internal class GetCalendarHandler : IQueryHandler<GetCalendar, CalendarDto>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetCalendarHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<CalendarDto> HandleAsync(GetCalendar query, CancellationToken cancellationToken = default)
        {
            if (query.Nights < 0)
                throw new BookingInvalidNightsException();

            var rental = await _rentalRepository.GetAsync(query.RentalId);

            if (rental is null)
            {
                throw new RentalNotExistException(query.RentalId);
            }

            return rental.AsDto(query.Start, query.Nights);
        }
    }
}
