using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands.Handlers
{
    internal class AddBookingHandler : ICommandHandler<AddBooking, int>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public AddBookingHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<int> HandleAsync(AddBooking command, CancellationToken cancellationToken = default)
        {
            if (command.Nights <= 0)
                throw new BookingInvalidNightsException();

            var rental = await _rentalRepository.GetAsync(command.RentalId);

            if (rental is null)
            {
                throw new RentalNotExistException(command.RentalId);
            }

            var newBooking = rental.CreateBooking(command.Start, command.Nights);

            await _bookingRepository.AddAsync(newBooking);

            return newBooking.Id;
        }
    }
}
