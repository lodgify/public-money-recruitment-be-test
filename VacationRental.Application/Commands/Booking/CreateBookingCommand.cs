using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Commands.Booking
{
    public class CreateBookingCommand
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<CreateBookingCommand> _logger;

        public CreateBookingCommand(IRentalRepository rentalRepository, IBookingRepository bookingRepository, ILogger<CreateBookingCommand> logger)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BookingCommandRequest bookingRequest)
        {
            var rental = _rentalRepository.Get(new RentalId(bookingRequest.RentalId));
            var bookings = _bookingRepository.GetByRentalId(rental.Id);

            var newBooking = rental.Book(bookings, new BookingPeriod(bookingRequest.Start, bookingRequest.Nights));

            _bookingRepository.Add(newBooking);
            _logger.LogInformation($"Booking for the rental '{rental.Id.Id}' from '{bookingRequest.Start}' for '{bookingRequest.Nights}' nights has been created");
        }
    }
}
