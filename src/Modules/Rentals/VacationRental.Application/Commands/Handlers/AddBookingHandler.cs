using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Exceptions;
using VacationRental.Core.Entities;
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
                throw new ApplicationException("Nigts must be positive");

            var rental = _rentalRepository.Get(command.RentalId);

            if (rental is null)
            {
                throw new RentalNotFoundException(command.RentalId);
            }

            var rentalBookings = _bookingRepository.GetAll(command.RentalId);

            for (var i = 0; i < command.Nights; i++)
            {
                var count = 0;
                foreach (var booking in rentalBookings)
                {
                    if (booking.IsNotAvailable(command.Start, command.Nights))
                    {
                        count++;
                    }
                }

                if (count >= rental.Units)
                    throw new BookingNotAvailableException();
            }

            var newBooking = new Booking(command.RentalId, command.Start, command.Nights);

            _bookingRepository.Add(newBooking);

            return newBooking.Id;
        }
    }
}
